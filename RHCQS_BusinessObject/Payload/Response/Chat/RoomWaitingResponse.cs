using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_BusinessObject.Payload.Response.Chat
{
    public class RoomWaitingResponse
    {
        public Guid Id { get; set; }
        public string AvatarStaff {  get; set; }
        public string StaffName { get; set; }
        public string MessageContext { get; set; }
        public bool IsRead { get; set; }
    }
}
