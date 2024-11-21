using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RHCQS_BusinessObject.Payload.Request.Chat;
using RHCQS_BusinessObject.Payload.Response.Chat;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System.Collections.Concurrent;

namespace RHCQS_BE.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IAccountService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private static readonly ConcurrentDictionary<string, ConcurrentBag<string>> UserConnections = new();
        public IAccountService _accountService;
        //private static readonly ConcurrentDictionary<Guid, ConcurrentBag<Message>> PrivateMessages = new();

        public ChatHub(IMessageService messageService, IAccountService userService, IUnitOfWork unitOfWork, IAccountService accountService)
        {
            _messageService = messageService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }

        private static Dictionary<string, List<string>> roomUsers = new();


        //[Authorize]
        //public async Task SendMessageToRoom(Guid roomId, string messageContent)
        //{
        //    var user = await _userService.GetCurrentLoginUser();
        //    var userId = user.Userid;
        //    if (userId == Guid.Empty)
        //    {
        //        throw new HubException("Invalid user ID.");
        //    }

        //    var room = await _messageService.GetChatRoomByIdAsync(roomId);
        //    if (room == null)
        //    {
        //        throw new HubException("Room does not exist.");
        //    }

        //    var createMessage = new MessageRequest
        //    {
        //        MessageContent = messageContent,
        //        RoomId = roomId,
        //        CreatedBy = userId
        //    };

        //    var message = await _messageService.CreateMessage(createMessage);
        //    if (message.CreatedBy! == null)
        //    {
        //        return;
        //    }

        //    await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userId.ToString(), messageContent);
        //}
        //[Authorize]
        //public async Task JoinRoom(Guid chatRoomId)
        //{
        //    var chatRoom = await _messageService.GetChatRoomByIdAsync(chatRoomId);
        //    if (chatRoom == null)
        //    {
        //        throw new HubException("Invalid chatRoomId.");
        //    }

        //    await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());

        //    var messages = await _messageService.GetMessagesByChatRoomId(chatRoomId);

        //    foreach (var message in messages)
        //    {
        //        await Clients.Caller.SendAsync("ReceiveMessage", message.CreatedBy.ToString(), message.Content);
        //    }
        //}
        //[Authorize]
        //public async Task LeaveRoom(Guid chatRoomId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        //}

        public async Task StartChatWithStaff(Guid customerId, Guid saleId, string initialMessage)
        {
            var context = _unitOfWork.GetRepository<Room>();
            var existingRoom = await context.FirstOrDefaultAsync(r => r.SenderId == customerId && r.SenderId == saleId);

            if (existingRoom != null)
            {
                await JoinRoom(existingRoom.Id, customerId.ToString());
                await SendMessageToRoom(existingRoom.Id, customerId.ToString(), initialMessage);
            }
            else
            {
                var room = new Room
                {
                    Id = Guid.NewGuid(),
                    SenderId = customerId,
                    ReceiverId = saleId,
                    InsDate = DateTime.Now,
                };

                await context.InsertAsync(room);
                await _unitOfWork.CommitAsync();

                await Clients.User(saleId.ToString()).SendAsync("ReceiveRoomNotification", room.Id.ToString());
                await SendMessageToRoom(room.Id, customerId.ToString(), initialMessage);
            }
        }

        public async Task JoinRoom(Guid roomId, string username)
        {
            if (!roomUsers.ContainsKey(roomId.ToString()))
                roomUsers[roomId.ToString()] = new List<string>();

            roomUsers[roomId.ToString()].Add(Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Group(roomId.ToString()).SendAsync("UserJoined", username);
        }

        public async Task SendMessageToRoom(Guid roomId, string user, string message)
        {
            try
            {
                var room = await _unitOfWork.GetRepository<Room>().FirstOrDefaultAsync(predicate: r => r.Id == roomId);
                if (room != null)
                {
                    Guid senderId;
                    var account = await _accountService.SearchAccountsByNameAsync(user);

                    //if (user == "Staff")
                    //{
                    //    senderId = Guid.Parse("BFA97975-1915-46A0-B185-ED881C8C953F");
                    //}
                    //else
                    //{
                    //    senderId = Guid.Parse("C9993BBF-9125-466D-BECA-E69CE3DE4A36");
                    //}

                    // Tạo tin nhắn mới
                    var newMessage = new Message
                    {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        MessageContent = message,
                        CreatedBy = account.Id,
                        SendAt = DateTime.Now
                    };

                    await _unitOfWork.GetRepository<Message>().InsertAsync(newMessage);
                    var saveResult = await _unitOfWork.CommitAsync();

                    if (saveResult > 0)
                    {
                        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", user, message, roomId);
                    }
                    else
                    {
                        Console.WriteLine("Failed to save message to database");
                    }
                }
                else
                {
                    Console.WriteLine("Room not found");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var room in roomUsers)
            {
                room.Value.Remove(Context.ConnectionId);
                if (room.Value.Count == 0)
                    roomUsers.Remove(room.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
