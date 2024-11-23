using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using RHCQS_BusinessObject.Payload.Response.Chat;
using RHCQS_BusinessObjects;
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
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IRoomService> _logger;

        public RoomService(IUnitOfWork unitOfWork, ILogger<IRoomService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<RoomWaitingResponse>> ListRoomWaiting(Guid accountId)
        {
            var accountInfo = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                            predicate: a => a.Id == accountId);
            if (accountInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.AccountIdError);
            }

            List<RoomWaitingResponse> roomWaitingList = new List<RoomWaitingResponse>();

            var rooms = await _unitOfWork.GetRepository<Room>().GetListAsync(
                                predicate: r => r.SenderId == accountId || r.ReceiverId == accountId);

            foreach (var room in rooms)
            {
                var latestMessage = await _unitOfWork.GetRepository<Message>().FirstOrDefaultAsync(
                    predicate: m => m.RoomId == room.Id,
                    orderBy: m => m.OrderByDescending(x => x.SendAt));

                var staffId = room.SenderId == accountId ? room.ReceiverId : room.SenderId;
                var staffAccount = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(a => a.Id == staffId);

                roomWaitingList.Add(new RoomWaitingResponse
                {
                    Id = room.Id,
                    AvatarStaff = staffAccount?.ImageUrl,
                    StaffName = staffAccount?.Username,
                    MessageContext = latestMessage?.MessageContent,
                    IsRead = false
                });
            }


            return roomWaitingList;
        }

        public async Task<MessageRoomReponse> ListMessageFromRoom(Guid roomId)
        {
            var roomInfo = await _unitOfWork.GetRepository<Room>().FirstOrDefaultAsync(
                                predicate: r => r.Id == roomId,
                                include: r => r.Include(r => r.Messages));
            if (roomInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Room_Not_Found);
            }

            Guid staffId = (Guid)roomInfo.SenderId == (Guid)roomInfo.ReceiverId ? (Guid)roomInfo.SenderId : (Guid)roomInfo.ReceiverId;

            var staffAccount = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(a => a.Id == staffId);
            if (staffAccount == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, "Staff not found");
            }

            var response = new MessageRoomReponse
            {
                StaffAvatar = staffAccount.ImageUrl,  
                StaffName = staffAccount.Username,   
                MessageRooms = roomInfo.Messages.Select(m => new MessageRoom
                {
                    UserId = (Guid)m.CreatedBy,
                    MessageContext = m.MessageContent,
                    SendAt = m.SendAt,
                    IsRead = false
                }).ToList()
            };

            return response;
        }
    }
}
