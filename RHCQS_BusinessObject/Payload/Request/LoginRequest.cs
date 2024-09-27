using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; } = null!;
    }
}
