using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class AccountRequest
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
    }
}
