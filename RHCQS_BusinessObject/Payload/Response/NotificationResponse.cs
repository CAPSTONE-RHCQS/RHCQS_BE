using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response
{
    public class NotificationResponse
    {
        public string Body { get; set; }
        public string DeviceToken { get; set; }
        public string Timestamp { get; set; }
        public string Title { get; set; }

    }
}
