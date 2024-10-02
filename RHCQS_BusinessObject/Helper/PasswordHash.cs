using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Helper
{
    public static class PasswordHash
    {
        public static bool VerifyPassword(string providedPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedHash);
        }

        public static string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }
    }
}
