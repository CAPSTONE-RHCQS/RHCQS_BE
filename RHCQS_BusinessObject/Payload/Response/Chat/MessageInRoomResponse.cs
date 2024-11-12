using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Chat
{
    public class MessageInRoomResponse
    {
        public Guid messageId { get; set; }
        public string Content { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? Avatar { get; set; }
    }
}
