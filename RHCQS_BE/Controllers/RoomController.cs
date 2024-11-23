using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        #region ListRoomWaiting
        /// <summary>
        /// List room for actor
        /// 
        /// Role: CUSTOMER - SALESSTAFF
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        #endregion
        //[Authorize(Roles = "Customer, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Room.RoomListWaitingEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListRoomWaiting(Guid accountId)
        {
            var list = await _roomService.ListRoomWaiting(accountId);
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }

        #region ListMessageFromRoom
        /// <summary>
        /// History chat in room
        /// 
        /// Role: CUSTOMER - SALESSTAFF
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        #endregion
        //[Authorize(Roles = "Customer, SalesStaff")]
        [HttpGet(ApiEndPointConstant.Room.ListMessageEndpoint)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListMessageFromRoom(Guid roomId)
        {
            var list = await _roomService.ListMessageFromRoom(roomId);
            var result = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult()
            {
                Content = result,
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
    }
}
