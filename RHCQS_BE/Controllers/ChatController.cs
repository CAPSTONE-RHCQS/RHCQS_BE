//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using RHCQS_Services.Implement;
//using System.Threading.Tasks;

//namespace RHCQS_BE.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ChatController : ControllerBase
//    {
//        //private readonly IHubContext<ChatService> _chatHubContext;

//        //public ChatController(IHubContext<ChatService> chatHubContext)
//        //{
//        //    _chatHubContext = chatHubContext;
//        //}

//        //[HttpPost("SendMessageToUser")]
//        //public async Task<IActionResult> SendMessageToUser([FromQuery] Guid fromUserId, [FromQuery] Guid toUserId, [FromBody] string message)
//        //{
//        //    await _chatHubContext.Clients.User(toUserId.ToString()).SendAsync("ReceiveMessage", fromUserId, message);
//        //    return Ok("Message sent to user");
//        //}

//        //[HttpPost("SendMessageToRoom")]
//        //public async Task<IActionResult> SendMessageToRoom([FromQuery] string roomName, [FromQuery] Guid fromUserId, [FromBody] string message)
//        //{
//        //    await _chatHubContext.Clients.Group(roomName).SendAsync("ReceiveRoomMessage", fromUserId, message);
//        //    return Ok("Message sent to room");
//        //}

//        ////[HttpPost("JoinRoom")]
//        ////public async Task<IActionResult> JoinRoom([FromQuery] string roomName)
//        ////{
//        ////    var connectionId = Context.ConnectionId; // Retrieve connection ID
//        ////    await _chatHubContext.Groups.AddToGroupAsync(connectionId, roomName);
//        ////    return Ok($"Joined room {roomName}");
//        ////}

//        ////[HttpPost("LeaveRoom")]
//        ////public async Task<IActionResult> LeaveRoom([FromQuery] string roomName)
//        ////{
//        ////    var connectionId = Context.ConnectionId; // Retrieve connection ID
//        ////    await _chatHubContext.Groups.RemoveFromGroupAsync(connectionId, roomName);
//        ////    return Ok($"Left room {roomName}");
//        ////}
//        //[HttpPost("JoinRoom")]
//        //public async Task<IActionResult> JoinRoom([FromQuery] string roomName, [FromQuery] string connectionId)
//        //{
//        //    await _chatHubContext.Groups.AddToGroupAsync(connectionId, roomName);
//        //    return Ok($"Joined room {roomName}");
//        //}

//        //[HttpPost("LeaveRoom")]
//        //public async Task<IActionResult> LeaveRoom([FromQuery] string roomName, [FromQuery] string connectionId)
//        //{
//        //    await _chatHubContext.Groups.RemoveFromGroupAsync(connectionId, roomName);
//        //    return Ok($"Left room {roomName}");
//        //}

//    }
//}
