using RHCQS_BusinessObject.Payload.Response.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IRoomService
    {
        Task<List<RoomWaitingResponse>> ListRoomWaiting(Guid accountId);
        Task<MessageRoomReponse> ListMessageFromRoom(Guid roomId);
    }
}
