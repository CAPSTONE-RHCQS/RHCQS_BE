using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Chat
{
    public class MessageRoomReponse
    {
        public string StaffAvatar { get; set; }
        public string StaffName { get; set; }
        public List<MessageRoom> MessageRooms { get; set; }
    }

    public class MessageRoom
    {
        public Guid UserId {  get; set; }
        public string MessageContext { get; set; }
        public DateTime? SendAt { get; set; }
        public bool IsRead { get; set; }
    }
}
