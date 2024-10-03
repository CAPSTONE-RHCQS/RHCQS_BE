using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc phải có.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ(phải theo dạng @gmail.com).")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password là bắt buộc phải có.")]
        public string Password { get; set; } = null!;
    }
}
