using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class AccountResponse
    {
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? ImageUrl { get; set; }

        public string? PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public bool? Deflag { get; set; }

        public AccountResponse(Guid id, string? username, string? phoneNumber, DateOnly? dateOfBirth, string? passwordHash,
            string? email, string? imageUrl, bool? deflag, Guid roleId, DateTime? insDate, DateTime? upsDate)
        {
            Id = id;
            Username = username;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            PasswordHash = passwordHash;  // Fixed mismatch in parameter
            Email = email;
            ImageUrl = imageUrl;  // Fixed mismatch in parameter
            Deflag = deflag;
            RoleId = roleId;
            InsDate = insDate;
            UpsDate = upsDate;
        }
    }
}
