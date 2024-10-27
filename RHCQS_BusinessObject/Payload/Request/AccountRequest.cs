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
        [Required(ErrorMessage = "Id là bắt buộc phải có.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "RoleId là bắt buộc phải có.")]
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc phải có.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ(phải theo dạng @gmail.com).")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Username là bắt buộc phải có.")]
        [StringLength(50, ErrorMessage = "Username không được dài quá 50 ký tự.")]
        public string? Username { get; set; }

        [Url(ErrorMessage = "ImageUrl phải là một URL hợp lệ.")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Password là bắt buộc phải có.")]
        [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự.")]
        public string? PasswordHash { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? InsDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpsDate { get; set; }

        public bool? Deflag { get; set; }
    }
    public class AccountRequestForUpdate
    {

        [StringLength(50, ErrorMessage = "Username không được dài quá 50 ký tự.")]
        public string? Username { get; set; }

        [Url(ErrorMessage = "ImageUrl phải là một URL hợp lệ.")]
        public string? ImageUrl { get; set; }

        [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự.")]
        public string? PasswordHash { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        public bool? Deflag { get; set; }
    }
    public class AccountRequestForUpdateProfile
    {
        [StringLength(50, ErrorMessage = "Username không được dài quá 50 ký tự.")]
        public string? Username { get; set; }

        [Url(ErrorMessage = "ImageUrl phải là một URL hợp lệ.")]
        public string? ImageUrl { get; set; }

        [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự.")]
        public string? PasswordHash { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

    }
    public class DeTokenRequest
    {
        public string Token;
    }
}
