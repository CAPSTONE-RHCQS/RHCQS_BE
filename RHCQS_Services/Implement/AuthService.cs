﻿using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace RHCQS_Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IGmailSenderService _gmail;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IGmailSenderService gmail)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _gmail = gmail;
        }
        public async Task<Account> GetAccountByEmail(string email, string password)
        {
            var account = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                predicate: x => x.Email.Equals(email),
                include: q => q.Include(x => x.Role));

            if (account == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Account
                );
            }
            if (!PasswordHash.VerifyPassword(password, account.PasswordHash))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Forbidden,
                    AppConstant.ErrMessage.InvalidPassword
                );
            }
            return account;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var account = await GetAccountByEmail(email, password);
            if (account != null && account.Role.RoleName == AppConstant.Role.Customer)
            {
                var isEmailVerified = await IsEmailVerifiedAsync(email);
                if (!isEmailVerified)
                {
                    Task.Run(() => SendVerificationEmailAsync(email));
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Unauthorized,
                        AppConstant.ErrMessage.EmailNotVerified
                    );
                }
            }

            if ((bool)!account.Deflag)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Forbidden,
                    AppConstant.ErrMessage.AccountInActive
                );
            }

            return GenerateJwtToken(account);
        }
        public async Task<Account> RegisterAsync(RegisterRequest registerRequest, UserRoleForRegister selectedrole)
        {
            if (registerRequest == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var existingAccount = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                x => x.Email.Equals(registerRequest.Email));

            if (existingAccount != null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.EmailExists
                );
            }

            var existingPhoneNumber = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                x => x.PhoneNumber.Equals(registerRequest.PhoneNumber)
            );

            if (existingPhoneNumber != null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.PhoneNumberExists
                );
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.PasswordMismatch
                );
            }

            var roleName = selectedrole.ToString();
            var role = await _unitOfWork.GetRepository<Role>().FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.RoleNotFound
                );
            }
            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                PasswordHash = PasswordHash.HashPassword(registerRequest.Password),
                Username = registerRequest.Email,
                InsDate = LocalDateTime.VNDateTime(),
                UpsDate = LocalDateTime.VNDateTime(),
                RoleId = role.Id,
                Deflag = true
            };
            if (roleName == UserRoleForRegister.Customer.ToString())
            {
                var newCustomer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Email = registerRequest.Email,
                    PhoneNumber = registerRequest.PhoneNumber,
                    Username = registerRequest.Email,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime(),
                    Deflag = true,
                    AccountId = newAccount.Id
                };

                await _unitOfWork.GetRepository<Customer>().InsertAsync(newCustomer);
            }

            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.CreateAccountError
                );
            }
            if (roleName == UserRoleForRegister.Customer.ToString())
            {
                Task.Run(() => SendVerificationEmailAsync(registerRequest.Email));
            }
            return newAccount;
        }
        public async Task<string> RefreshTokenAsync(string expiredToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var keyString = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var principal = tokenHandler.ValidateToken(expiredToken, validationParameters, out securityToken);

            if (securityToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Unauthorized,
                    AppConstant.ErrMessage.InvalidToken
                );
            }

            var accountId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(role))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Unauthorized,
                    AppConstant.ErrMessage.InvalidToken
                );
            }
            var expirationTime = jwtToken.ValidTo;
            if (expirationTime > DateTime.UtcNow)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.Not_Token_expired
                );
            }
            var account = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                predicate: x => x.Id.ToString() == accountId,
                include: q => q.Include(x => x.Role)
            );

            if (account == null || (bool)!account.Deflag)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Unauthorized,
                    AppConstant.ErrMessage.AccountInActive
                );
            }

            return GenerateJwtToken(account);

        }
        private string GenerateJwtToken(Account account)
        {
            if (account == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Name, account.Username ?? ""),
                    new Claim(ClaimTypes.MobilePhone, account.PhoneNumber ?? ""),
                    new Claim(ClaimTypes.Role, account.Role.RoleName),
                    new Claim("ImgUrl", account.ImageUrl ?? "")
                };

            var keyString = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = LocalDateTime.VNDateTime().AddDays(1);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public TokenResponse DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                var tokenResponse = new TokenResponse
                {
                    NameIdentifier = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value!,
                    Name = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value!,
                    MobilePhone = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone")?.Value!,
                    Role = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value!,
                    ImgUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxzT5167I50_3KwBagkh8DPQmmEEj0ec0ENA&s",
                    Exp = long.Parse(jsonToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value ?? "0")
                };

                return tokenResponse;
            }

            return null;
        }
        private async Task SendVerificationEmailAsync(string email)
        {
            try
            {
                var auth = FirebaseAuth.DefaultInstance;

                // Try to get the user by email
                UserRecord user = null;
                try
                {
                    user = await auth.GetUserByEmailAsync(email);
                }
                catch (FirebaseAuthException ex)
                {
                    // Handle user not found
                    Console.WriteLine($"User with email {email} not found. Creating new user.");

                    var userRecordArgs = new UserRecordArgs
                    {
                        Email = email,
                        EmailVerified = false,
                        Disabled = false,
                    };

                    // Create a new user
                    var userRecord = await auth.CreateUserAsync(userRecordArgs);
                    Console.WriteLine($"User created with UID: {userRecord.Uid}");

                    // Assign the created user to the 'user' variable
                    user = userRecord;
                }

                // Generate the email verification link
                var link = await auth.GenerateEmailVerificationLinkAsync(email);

                // Construct the email body
                var emailBody = $@"
                Xin chào {email},
                Nhấn vào link để xác thực email của bạn:
                {link}
                Nếu bạn không yêu cầu xác minh địa chỉ này, bạn có thể bỏ qua email này.

                Cảm ơn,
                RHCQS team";


                // Send the email via Gmail API or your custom email service
                await _gmail.SendEmailAsync(email, "Xác thực email", emailBody, null);

                Console.WriteLine($"Verification email sent to: {email}");
            }
            catch (FirebaseAuthException ex)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    $"Failed to send verification email: {ex.Message}"
                );
            }
        }


        public async Task<bool> IsEmailVerifiedAsync(string email)
        {
            try
            {
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

                return userRecord.EmailVerified;
            }
            catch (FirebaseAuthException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}
