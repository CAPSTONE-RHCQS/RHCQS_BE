using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload
{
    public class SendEmailAndNotiReponse
    {
        public string Email { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerName { get; set; }
    }
}
