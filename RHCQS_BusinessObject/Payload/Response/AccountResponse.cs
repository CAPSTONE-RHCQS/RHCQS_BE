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
        public string? RoleName { get; set; }

        public string? Username { get; set; }

        public string? ImageUrl { get; set; }

        public string? PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpsDate { get; set; }

        public bool? Deflag { get; set; }

        public AccountResponse(Guid id, string? username, string? phoneNumber, DateOnly? dateOfBirth, string? passwordHash,
            string? email, string? imageUrl, bool? deflag,string ? rolename, Guid roleId, DateTime? insDate, DateTime? upsDate)
        {
            Id = id;
            Username = username;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            PasswordHash = passwordHash;
            Email = email;
            ImageUrl = imageUrl;
            Deflag = deflag;
            RoleName = rolename;
            RoleId = roleId;
            InsDate = insDate;
            UpsDate = upsDate;
        }

        public AccountResponse(Guid id, string? username, string? phoneNumber, DateOnly? dateOfBirth, string? passwordHash, 
            string? email, bool? deflag, string? roleName, Guid roleId, DateTime? insDate, DateTime? upsDate)
        {
            Id = id;
            Username = username;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            PasswordHash = passwordHash;
            Email = email;
            Deflag = deflag;
            RoleName = roleName;
            RoleId = roleId;
            InsDate = insDate;
            UpsDate = upsDate;
        }
    }
}
