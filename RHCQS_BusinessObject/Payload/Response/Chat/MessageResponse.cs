using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Chat
{
    public class MessageResponse
    {
        public Guid Id { get; set; }

        public string? MessageContent { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime? SendAt { get; set; }

        public Guid RoomId { get; set; }
    }
}
