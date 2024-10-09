using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class TokenResponse
    {
        public string NameIdentifier { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Role { get; set; }
        public string ImgUrl { get; set; }
        public long Exp { get; set; } 
    }

}
