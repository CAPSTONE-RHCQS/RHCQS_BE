using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "expiredToken là bắt buộc.")]
        public string expiredTokenToken { get; set; }
    }
}
