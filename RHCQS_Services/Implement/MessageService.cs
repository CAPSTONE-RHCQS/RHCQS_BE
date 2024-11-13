using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Response.Chat;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IUnitOfWork unitOfWork, ILogger<MessageService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ChatRoomResponse> GetChatRoomByIdAsync(Guid chatRoomId)
        {
            var chatroom = await _unitOfWork.GetRepository<Room>().FirstOrDefaultAsync(
                 predicate: x => x.Id == chatRoomId);
            var response = new ChatRoomResponse(
                           chatroom.Id,
                           chatroom.SenderId,
                           chatroom.ReceiverId,
                           chatroom.InsDate);

            return response;
        }
    }
}
