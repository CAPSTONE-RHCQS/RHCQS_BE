﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RHCQS_BusinessObject.Payload.Request;
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

            if (account == null || !VerifyPassword(password, account.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            return account;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            var account = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(predicate: x =>
                x.Email.Equals(email), include: q => q.Include(x => x.Role));

            if (account == null || !VerifyPassword(password, account.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials or account not found.");
            }

            return GenerateJwtToken(account);
        }
        public async Task<Account> RegisterAsync(RegisterRequest registerRequest, UserRoleForRegister selectedrole)
        {
            var existingAccount = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                x => x.Email.Equals(registerRequest.Email));

            if (existingAccount != null)
            {
                return null;
            }
            var roleName = selectedrole.ToString();
            var role = await _unitOfWork.GetRepository<Role>().FirstOrDefaultAsync(r => r.RoleName == roleName);
            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                Email = registerRequest.Email,
                PasswordHash = HashPassword(registerRequest.Password),
                Username = selectedrole.ToString(),
                InsDate = DateTime.UtcNow,
                UpsDate = DateTime.UtcNow,
                RoleId = role.Id,
                Deflag = true
            };

            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new Exception("Commit failed, no rows affected.");
            }

            return newAccount;
        }
        private bool VerifyPassword(string providedPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }
        private string GenerateJwtToken(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
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
            var expires = DateTime.Now.AddMinutes(30);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
