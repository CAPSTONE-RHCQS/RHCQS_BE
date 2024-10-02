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
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "ConfirmPassword is required")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRoleForRegister
    {
        Customer,
        Manager,
        SalesStaff,
        DesignStaff
    }
}
