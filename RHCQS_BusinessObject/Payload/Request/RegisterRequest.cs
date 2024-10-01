using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
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
