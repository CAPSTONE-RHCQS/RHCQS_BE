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
using System.Net.Http.Headers;
using RHCQS_BusinessObject.Payload.Response.HouseDesign;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.HaveDrawing;

namespace RHCQS_Services.Implement
{
    public class HouseDesignVersionService : IHouseDesignVersionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseDesignVersionService> _logger;
        private readonly Cloudinary _cloudinary;
        private readonly IMediaService _mediaService;

        public HouseDesignVersionService(IUnitOfWork unitOfWork,
            ILogger<HouseDesignVersionService> logger,
            Cloudinary cloudinary,
            IConfiguration configuration,
            IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cloudinary = cloudinary;
            _mediaService = mediaService;
        }

        public async Task<HouseDesignVersionItemResponse> GetDetailVersionById(Guid versionId)
        {
            var version = await _unitOfWork.GetRepository<HouseDesignVersion>()
                                           .FirstOrDefaultAsync(predicate: v => v.Id == versionId,
                                                                include: v => v.Include(v => v.Media));

            if (version == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.House_Design_Not_Found);
            }

            var fileUrl = version.Media?.FirstOrDefault(m => m.HouseDesignVersionId == versionId)!.Url ?? "Chưa hoàn thành";

            var response = new HouseDesignVersionItemResponse(
                id: version.Id,
                name: version.Name,
                version: version.Version,
                fileUrl: fileUrl,
                insDate: version.InsDate,
                note: version.Note,
                reason: version.Reason,
                relatedDrawingId: version.RelatedDrawingId,
                previousDrawingId: version.PreviousDrawingId
            );

            return response;
        }

        public async Task<bool> CreateHouseDesignVersion(Guid accountId, HouseDesignVersionRequest request)
        {
            try
            {
                var staffInfo = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                .FirstOrDefaultAsync(x => x.Account!.Id == accountId && x.Id == request.HouseDesignDrawingId,
                include: x => x.Include(x => x.Account!));
                if (staffInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Not_Access_DesignDrawing);
                }

                var availableDrawing = await _unitOfWork.GetRepository<HouseDesignVersion>().FirstOrDefaultAsync(
                                                v => v.HouseDesignDrawingId == request.HouseDesignDrawingId,
                                                include: x => x.Include(v => v.HouseDesignDrawing),
                                                orderBy: v => v.OrderByDescending(v => v.Version));
                if (availableDrawing == null)
                {
                    var itemDesign = new HouseDesignVersion
                    {
                        Id = Guid.NewGuid(),
                        Name = request.Name,
                        Version = 1.0,
                        InsDate = LocalDateTime.VNDateTime(),
                        HouseDesignDrawingId = request.HouseDesignDrawingId,
                        Note = "",
                        PreviousDrawingId = request.PreviousDrawingId ?? null,
                        RelatedDrawingId = request.RelatedDrawingId ?? null,
                        UpsDate = LocalDateTime.VNDateTime(),
                        Deflag = false,
                    };

                    await _unitOfWork.GetRepository<HouseDesignVersion>().InsertAsync(itemDesign);

                    //Update status PENDING -> REVIEWING
                    var drawingInfo = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                                            .FirstOrDefaultAsync(predicate: x => x.Id == request.HouseDesignDrawingId);

                    if (drawingInfo == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.House_Design_Not_Found);
                    }
                    drawingInfo.Status = AppConstant.HouseDesignStatus.REVIEWING;

                    _unitOfWork.GetRepository<HouseDesignDrawing>().UpdateAsync(drawingInfo);

                    //Create media save file
                    var itemMedia = new Medium
                    {
                        Id = Guid.NewGuid(),
                        Name = request.Name,
                        HouseDesignVersionId = itemDesign.Id,
                        Url = request.FileUrl,
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime()
                    };
                    await _unitOfWork.GetRepository<Medium>().InsertAsync(itemMedia);
                }
                else
                {
                    var itemDesignUpdate = new HouseDesignVersion
                    {
                        Id = Guid.NewGuid(),
                        Name = availableDrawing!.Name,
                        Version = availableDrawing.Version + 1,
                        InsDate = LocalDateTime.VNDateTime(),
                        HouseDesignDrawingId = availableDrawing.HouseDesignDrawingId,
                        Note = null,
                        UpsDate = LocalDateTime.VNDateTime(),
                        RelatedDrawingId = availableDrawing?.RelatedDrawingId,
                        PreviousDrawingId = availableDrawing?.PreviousDrawingId,
                    };

                    //Update status in house desgin draw previous
                    availableDrawing!.HouseDesignDrawing.Status = AppConstant.Status.UPDATED;

                    _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(availableDrawing);

                    await _unitOfWork.GetRepository<HouseDesignVersion>().InsertAsync(itemDesignUpdate);

                    var itemMedia = new Medium
                    {
                        Id = Guid.NewGuid(),
                        Name = request.Name,
                        HouseDesignVersionId = itemDesignUpdate.Id,
                        Url = request.FileUrl,
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime()
                    };
                    await _unitOfWork.GetRepository<Medium>().InsertAsync(itemMedia);
                }
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (isSuccessful)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }

        }

        public async Task<bool> UploadDesignDrawing(List<IFormFile> files, Guid versionId)
        {
            Medium itemMedia = null;
            if (files == null || files.Count == 0)
            {
                return false;
            }

            var itemDrawing = await _unitOfWork.GetRepository<HouseDesignVersion>()
                                               .FirstOrDefaultAsync(predicate: x => x.Id == versionId,
                                                                    include: x => x.Include(x => x.HouseDesignDrawing));
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

                var publicId = $"{itemDrawing.Name + "_" + itemDrawing.Version}" + LocalDateTime.VNDateTime().ToString() ?? Path.GetFileNameWithoutExtension(file.FileName);

                var imageUrl = await _mediaService.UploadImageAsync(file, "HouseDesignDrawing", publicId);

                if (imageUrl == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.FailUploadDrawing);
                }

                itemMedia = new Medium()
                {
                    Id = Guid.NewGuid(),
                    HouseDesignVersionId = itemDrawing.Id,
                    Name = itemDrawing.Name,
                    Url = imageUrl,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime()
                };
                await _unitOfWork.GetRepository<Medium>().InsertAsync(itemMedia);
                itemDrawing.HouseDesignDrawing.Status = "Processing";
            }

            itemDrawing.HouseDesignDrawing.Status = itemMedia!.Url != null ? "Finished" : "Processing";
            itemDrawing.UpsDate = LocalDateTime.VNDateTime();

            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(itemDrawing);
            bool isUpdate = await _unitOfWork.CommitAsync() > 0;
            return isUpdate;
        }

        public async Task<bool> ApproveHouseDrawing(Guid Id, AssignHouseDrawingRequest request)
        {
            var drawingItem = await _unitOfWork.GetRepository<HouseDesignVersion>().FirstOrDefaultAsync(x => x.Id == Id,
                                                include: x => x.Include(x => x.HouseDesignDrawing));

            if (drawingItem == null) throw new AppConstant.MessageError((int)(AppConstant.ErrCode.Not_Found),
                                               AppConstant.ErrMessage.HouseDesignDrawing);

            if (request.Type == AppConstant.HouseDesignStatus.APPROVED)
            {
                drawingItem.HouseDesignDrawing.Status = AppConstant.HouseDesignStatus.APPROVED;
                drawingItem.Deflag = true;
                var designDrawing = await _unitOfWork.GetRepository<HouseDesignDrawing>().
                    FirstOrDefaultAsync(x => x.Id == drawingItem.HouseDesignDrawingId);
                designDrawing.Status = AppConstant.HouseDesignStatus.APPROVED;
            }
            else
            {
                drawingItem.HouseDesignDrawing.Status = AppConstant.HouseDesignStatus.UPDATING;
                drawingItem.Reason = request.Reason;
            }
            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(drawingItem);



            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }

        public async Task<string> ConfirmDesignDrawingFromCustomer(Guid versionId)
        {
            var designVersionInfo = await _unitOfWork.GetRepository<HouseDesignVersion>()
                                           .FirstOrDefaultAsync(predicate: x => x.Id == versionId,
                                                                include: x => x.Include(x => x.HouseDesignDrawing));
            if (designVersionInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.House_Design_Not_Found);
            }

            designVersionInfo.HouseDesignDrawing.Status = AppConstant.HouseDesignStatus.ACCEPTED;

            var nextStep = designVersionInfo.HouseDesignDrawing.Step + 1;

            if (nextStep == 2 || nextStep == 4)
            { 
                var nextStepDrawing = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                    .FirstOrDefaultAsync(predicate: x => x.ProjectId == designVersionInfo.HouseDesignDrawing.ProjectId && x.Step == nextStep);

                if (nextStepDrawing != null && nextStepDrawing.Status != AppConstant.HouseDesignStatus.ACCEPTED)
                {
                    nextStepDrawing.Status = AppConstant.HouseDesignStatus.PROCESSING;
                    _unitOfWork.GetRepository<HouseDesignDrawing>().UpdateAsync(nextStepDrawing);
                }
            }

            if (designVersionInfo.HouseDesignDrawing.Step == 4)
            {
                var contract = await _unitOfWork.GetRepository<Contract>()
                    .FirstOrDefaultAsync(predicate: x => x.ProjectId == designVersionInfo.HouseDesignDrawing.ProjectId);

                if (contract == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.Contract_Not_Found);
                }

                contract.Status = AppConstant.ContractStatus.COMPLETED;
                _unitOfWork.GetRepository<Contract>().UpdateAsync(contract);
            }

            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(designVersionInfo);

            string saveResult = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return saveResult;
        }


        public async Task<string> CommentDesignDrawingFromCustomer(Guid versionId, FeedbackHouseDesignDrawingRequest comment)
        {
            var designVersionInfo = await _unitOfWork.GetRepository<HouseDesignVersion>()
                                         .FirstOrDefaultAsync(predicate: x => x.Id == versionId,
                                                              include: x => x.Include(x => x.HouseDesignDrawing));
            if (designVersionInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.House_Design_Not_Found);
            }

            designVersionInfo.Note = comment.Note;
            designVersionInfo.HouseDesignDrawing.Status = AppConstant.HouseDesignStatus.UPDATING;
            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(designVersionInfo);

            string saveResutl = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return saveResutl;
        }
    }
}
