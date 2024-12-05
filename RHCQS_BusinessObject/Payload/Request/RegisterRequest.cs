using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc phải có.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ(phải theo dạng @gmail.com)")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber là bắt buộc phải có.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc phải có.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc phải có.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRoleForRegister
    {
        Customer,
        Manager
    }
    public enum UserRoleForManagerRegister
    {
        SalesStaff,
        DesignStaff
    }
}
