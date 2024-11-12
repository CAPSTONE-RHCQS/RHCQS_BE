using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request.Chat;
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

        public async Task<MessageResponse> CreateMessage(MessageRequest request)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                MessageContent = request.MessageContent,
                RoomId = request.RoomId,
                SendAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy
            };

            await _unitOfWork.GetRepository<Message>().InsertAsync(message);
            await _unitOfWork.CommitAsync();
            var response = new MessageResponse
            {
                Id = message.Id,
                MessageContent = message.MessageContent,
                CreatedBy = (Guid)message.CreatedBy,
                SendAt = message.SendAt,
                RoomId = message.RoomId
            };

            return response;
        }

        public async Task<List<MessageInRoomResponse>> GetMessagesByChatRoomId(Guid chatRoomId)
        {
            var messages = await _unitOfWork.GetRepository<Message>()
                                .GetListAsync(
                                    predicate: x => x.RoomId == chatRoomId,
                                     //include: x => x.Include(m => m.Account),
                                    orderBy: x => x.OrderBy(m => m.SendAt));

            var response = messages.Select(m => new MessageInRoomResponse
            {
                messageId = m.Id,
                Content = m.MessageContent,
                CreatedBy = m.CreatedBy,
                CreatedByUserName =  "Tom",
                CreatedDate = m.SendAt,
                CreatedTime = m.SendAt,
                Avatar = "no link"
            }).ToList();

            return response;
        }
    }
}
