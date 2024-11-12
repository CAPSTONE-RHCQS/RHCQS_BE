using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Request.Chat
{
    public class MessageRequest
    {

        public string? MessageContent { get; set; }
        public Guid CreatedBy { get; set; }

        public Guid RoomId { get; set; }
    }
}
