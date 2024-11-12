using RHCQS_BusinessObject.Payload.Request.Chat;
using RHCQS_BusinessObject.Payload.Response.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IMessageService
    {
        Task<ChatRoomResponse> GetChatRoomByIdAsync(Guid chatRoomId);
        Task<MessageResponse> CreateMessage(MessageRequest request);
        Task<List<MessageInRoomResponse>> GetMessagesByChatRoomId(Guid chatRoomId);
    }
}
