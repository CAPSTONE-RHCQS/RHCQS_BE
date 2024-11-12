using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RHCQS_Services.Interface;
using System.Collections.Concurrent;

namespace RHCQS_BE.Hub
{
    public class ChatHub
    {
        private readonly IMessageService _messageService;
        private readonly IAccountService _userService;
        private static readonly ConcurrentDictionary<string, ConcurrentBag<string>> UserConnections = new();
        //private static readonly ConcurrentDictionary<Guid, ConcurrentBag<Message>> PrivateMessages = new();

        public ChatHub(IMessageService messageService, IAccountService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        [Authorize]
        public async Task SendMessageToRoom(Guid roomId, string messageContent)
        {
            var user = await _userService.GetCurrentLoginUser();
            //var userId = user.Userid;
            //if (userId == Guid.Empty)
            //{
            //    throw new HubException("Invalid user ID.");
            //}

            var room = await _messageService.GetChatRoomByIdAsync(roomId);
            if (room == null)
            {
                throw new HubException("Room does not exist.");
            }

            //var createMessageModel = new CreateMessageModel
            //{
            //    MessageContent = messageContent,
            //    RoomId = roomId,
            //    CreatedBy = userId
            //};

            //var message = await _messageService.CreateMessage(createMessageModel);
            //if (message.CreatedBy == null)
            //{
            //    return;
            //}

            //await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userId.ToString(), messageContent);
        }
        [Authorize]
        public async Task JoinRoom(Guid chatRoomId)
        {
            var chatRoom = await _messageService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoom == null)
            {
                throw new HubException("Invalid chatRoomId.");
            }

            //await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());

            //var chatRoomDto = await _messageService.GetMessagesByChatRoomId(chatRoomId);
            //var messages = chatRoomDto.Messages;

            //foreach (var message in messages)
            //{
            //    await Clients.Caller.SendAsync("ReceiveMessage", message.CreatedBy.ToString(), message.Content);
            //}
        }
        [Authorize]
        public async Task LeaveRoom(Guid chatRoomId)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        }
    }
}
