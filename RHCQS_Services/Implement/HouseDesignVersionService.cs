using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Request;
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
    public class HouseDesignVersionService : IHouseDesignVersionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseDesignVersionService> _logger;

        public HouseDesignVersionService(IUnitOfWork unitOfWork, ILogger<HouseDesignVersionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> CreateHouseDesignVersion(HouseDesignVersionRequest request)
        {
            var staffInfo = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                .FirstOrDefaultAsync(x => x.AssignTask.Account.Id == request.AccountId && x.Id == request.HouseDesignDrawingId,
                include: x => x.Include(x => x.AssignTask)
                                .ThenInclude(x => x.Account));
            if (staffInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Access_DesignDrawing);
            }

            var availableDrawing = await _unitOfWork.GetRepository<HouseDesignVersion>().FirstOrDefaultAsync(
                                            v => v.HouseDesignDrawingId == request.HouseDesignDrawingId);
            if (availableDrawing == null)
            {
                var itemDesign = new HouseDesignVersion
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Version = 1.0,
                    Status = "Proccesing",
                    InsDate = DateTime.Now,
                    HouseDesignDrawingId = request.HouseDesignDrawingId,
                    Note = "",
                    FileUrl = request.FileUrl,
                    PreviousDrawingId = request.PreviousDrawingId ?? null,
                    RelatedDrawingId = request.RelatedDrawingId ?? null,
                    UpsDate = DateTime.Now,
                };

                await _unitOfWork.GetRepository<HouseDesignVersion>().InsertAsync(itemDesign);
            }
            else
            {
                var itemDesignUpdate = new HouseDesignVersion
                {
                    Id = Guid.NewGuid(),
                    Name = availableDrawing!.Name,
                    Version = availableDrawing.Version + 1,
                    Status = AppConstant.Status.PROCESSING,
                    InsDate = DateTime.Now,
                    HouseDesignDrawingId = availableDrawing.HouseDesignDrawingId,
                    Note = null,
                    FileUrl = request.FileUrl,
                    UpsDate= DateTime.Now,
                    RelatedDrawingId = availableDrawing?.RelatedDrawingId,
                    PreviousDrawingId = availableDrawing?.PreviousDrawingId,
                };

                //Update status in house desgin draw previous
                availableDrawing.Status = AppConstant.Status.UPDATED;

                 _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(availableDrawing);

                await _unitOfWork.GetRepository<HouseDesignVersion>().InsertAsync(itemDesignUpdate);
            }
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessful)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UploadDesignDrawing(HouseDesignVersionUpdateRequest request, Guid versionId)
        {
            var itemDrawing = await _unitOfWork.GetRepository<HouseDesignVersion>()
                                                .FirstOrDefaultAsync(x => x.Id == versionId);
            if (itemDrawing == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                AppConstant.ErrMessage.Not_Found_DesignDrawing);
            }

            itemDrawing.FileUrl = request.FileUrl ?? itemDrawing.FileUrl;
            itemDrawing.Status = itemDrawing.FileUrl != null ? "Finished" : "Processing";
            itemDrawing.RelatedDrawingId = itemDrawing.RelatedDrawingId;
            itemDrawing.PreviousDrawingId = itemDrawing.PreviousDrawingId;
            itemDrawing.UpsDate = DateTime.Now;

            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(itemDrawing);
            bool isUpdate = await _unitOfWork.CommitAsync() > 0;
            if (isUpdate)
            {
                return true;
            }
            return false;
        }
    }
}
