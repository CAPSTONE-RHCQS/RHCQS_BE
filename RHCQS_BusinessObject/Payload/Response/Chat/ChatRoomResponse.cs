using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Chat
{
    public class ChatRoomResponse
    {
        public ChatRoomResponse(Guid id, Guid? senderId, Guid? receiverId, DateTime? insDate)
        {
            Id = id;
            SenderId = senderId;
            SenderId = receiverId;
            InsDate = insDate;
        }
        public Guid Id { get; set; }

        public Guid? SenderId { get; set; }

        public Guid? ReceiverId { get; set; }

        public DateTime? InsDate { get; set; }
    }
}
