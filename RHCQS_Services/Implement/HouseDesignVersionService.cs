using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RHCQS_BusinessObject.Payload.Request.HouseDesign;

namespace RHCQS_Services.Implement
{
    public class HouseDesignVersionService : IHouseDesignVersionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseDesignVersionService> _logger;
        private readonly Cloudinary _cloudinary;

        public HouseDesignVersionService(IUnitOfWork unitOfWork, ILogger<HouseDesignVersionService> logger, Cloudinary cloudinary, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
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
                    Deflag = false,
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
                    UpsDate = DateTime.Now,
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

        public async Task<bool> UploadDesignDrawing(List<IFormFile> files, Guid versionId)
        {
            if (files == null || files.Count == 0)
            {
                return false;
            }

            var itemDrawing = await _unitOfWork.GetRepository<HouseDesignVersion>()
                                               .FirstOrDefaultAsync(x => x.Id == versionId);
            if (itemDrawing == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found,
                                                   AppConstant.ErrMessage.Not_Found_DesignDrawing);
            }

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                var publicId = $"{itemDrawing.Name + "_" + itemDrawing.Version}" + DateTime.Now.ToString() ?? Path.GetFileNameWithoutExtension(file.FileName);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = publicId,
                    Folder = "HouseDesignDrawing",
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.FailUploadDrawing);
                }

                itemDrawing.FileUrl = uploadResult.Url?.ToString() ?? itemDrawing.FileUrl;
                itemDrawing.Status = "Processing"; 
            }

            itemDrawing.Status = itemDrawing.FileUrl != null ? "Finished" : "Processing";
            itemDrawing.UpsDate = DateTime.Now;

            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(itemDrawing);
            bool isUpdate = await _unitOfWork.CommitAsync() > 0;
            return isUpdate;
        }

        public async Task<bool> AssignHouseDrawing(Guid Id, AssignHouseDrawingRequest request)
        {
            var drawingItem = await _unitOfWork.GetRepository<HouseDesignVersion>().FirstOrDefaultAsync(x => x.Id == Id);

            if (drawingItem == null) throw new AppConstant.MessageError((int)(AppConstant.ErrCode.Not_Found),
                                               AppConstant.ErrMessage.HouseDesignDrawing);

            if (request.Type == AppConstant.HouseDesignStatus.APPROVED)
            {
                drawingItem.Status = AppConstant.HouseDesignStatus.APPROVED;
                drawingItem.Deflag = true;
            }
            else
            {
                drawingItem.Status = AppConstant.HouseDesignStatus.UPDATED;
                drawingItem.Reason = request.Reason;
            }
            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(drawingItem);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
    }
}
