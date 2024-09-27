using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
                predicate: x => x.Email.Equals(email) && x.PasswordHash.Equals(password),
                include: q => q.Include(x => x.Role));

            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid phone or password.");
            }
            return account;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            var account = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(predicate: x =>
                x.Email.Equals(email) && x.PasswordHash.Equals(password), include: q => q.Include(x => x.Role));

            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials or account not found.");
            }

            return GenerateJwtToken(account);
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
