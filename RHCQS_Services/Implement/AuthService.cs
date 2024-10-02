using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
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
                PasswordHash = PasswordHash.HashPassword(registerRequest.Password),
                Username = registerRequest.Email,
                InsDate = DateTime.UtcNow,
                UpsDate = DateTime.UtcNow,
                RoleId = role.Id,
                Deflag = true
            };

            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.CreateAccountError
                );
            }

            return newAccount;
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
            var expires = DateTime.Now.AddDays(7);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
