using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class ChatService : Hub
    {
        // Use Guid for accountId instead of string
        private static readonly ConcurrentDictionary<Guid, string> Users = new ConcurrentDictionary<Guid, string>();

        // User connects
        public override Task OnConnectedAsync()
        {
            // Parse accountId as Guid
            if (Guid.TryParse(Context.GetHttpContext().Request.Query["accountId"], out Guid userId))
            {
                Users.TryAdd(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        // User disconnects
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Parse accountId as Guid
            if (Guid.TryParse(Context.GetHttpContext().Request.Query["accountId"], out Guid userId))
            {
                Users.TryRemove(userId, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        // Send message to a specific user
        public async Task SendMessageToUser(Guid fromUserId, Guid toUserId, string message)
        {
            if (Users.TryGetValue(toUserId, out string connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", fromUserId, message);
            }
        }

        // Send message to all users in a room
        public async Task SendMessageToRoom(string roomName, Guid fromUserId, string message)
        {
            await Clients.Group(roomName).SendAsync("ReceiveRoomMessage", fromUserId, message);
        }

        // Join a specific chat room
        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        // Leave a specific chat room
        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
