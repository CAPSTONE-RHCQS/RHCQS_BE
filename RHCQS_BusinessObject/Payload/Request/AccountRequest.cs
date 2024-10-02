using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class AccountRequest
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string? Username { get; set; }

        [Url(ErrorMessage = "ImageUrl must be a valid URL")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? PasswordHash { get; set; }

        [Phone(ErrorMessage = "Phone number is invalid")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? InsDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpsDate { get; set; }

        public bool? Deflag { get; set; }
    }
}
