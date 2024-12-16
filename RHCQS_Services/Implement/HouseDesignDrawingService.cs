
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request.HouseDesign;
using RHCQS_BusinessObject.Payload.Response.HouseDesign;
using RHCQS_BusinessObject.Payload.Response.Project;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_Services.Implement
{
    public class HouseDesignDrawingService : IHouseDesignDrawingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HouseDesignDrawingService> _logger;
        private readonly IMediaService _mediaService;

        public HouseDesignDrawingService(IUnitOfWork unitOfWork,
            ILogger<HouseDesignDrawingService> logger,
            IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mediaService = mediaService;
        }

        public async Task<IPaginate<ListHouseDesginResponse>> GetListHouseDesignDrawings(int page, int size)
        {
            var list = await _unitOfWork.GetRepository<HouseDesignDrawing>().GetList(
                        selector: x => new ListHouseDesginResponse(x.Id, x.ProjectId, x.Account.Username!,
                                                                      x.Name,
                                                                      x.Step,
                                                                      x.Status,
                                                                      x.Type,
                                                                      x.HaveDrawing, x.InsDate,
                                                                      x.HouseDesignVersions.Select(
                                                                          v => new HouseDesignVersionResponseList(
                                                                              v.Id,
                                                                              v.Name,
                                                                              v.Version,
                                                                              v.Media.Select(m => m.Url).FirstOrDefault(),
                                                                              v.InsDate,
                                                                              v.PreviousDrawingId,
                                                                              v.Note,
                                                                              v.Reason,
                                                                              v.Confirmed)).ToList()),
                        include: x => x.Include(x => x.HouseDesignVersions)
                                               .ThenInclude(x => x.Media)
                                               .Include(x => x.Account!),
                        orderBy: x => x.OrderBy(x => x.InsDate),
                        page: page,
                        size: size
                );
            return list;
        }

        public async Task<IPaginate<ListHouseDesginResponse>> GetListHouseDesignDrawingsForDesignStaff(int page, int size, Guid accountId)
        {
            var list = await _unitOfWork.GetRepository<HouseDesignDrawing>().GetList(
                        predicate: x => x.Account!.Id == accountId,
                         selector: x => new ListHouseDesginResponse(x.Id, x.ProjectId, x.Account!.Username!,
                                                                      x.Name,
                                                                      x.Step,
                                                                      x.Status,
                                                                      x.Type,
                                                                      x.HaveDrawing, x.InsDate,
                                                                      x.HouseDesignVersions.Select(
                                                                          v => new HouseDesignVersionResponseList(
                                                                              v.Id,
                                                                              v.Name,
                                                                              v.Version,
                                                                              v.Media.Select(m => m.Url).FirstOrDefault(),
                                                                              v.InsDate,
                                                                              v.PreviousDrawingId,
                                                                              v.Note,
                                                                              v.Reason, 
                                                                              v.Confirmed)).ToList()),
                        include: x => x.Include(x => x.HouseDesignVersions)
                                            .ThenInclude(x => x.Media)
                                            .Include(x => x.Account!),
                        orderBy: x => x.OrderBy(x => x.InsDate),
                        page: page,
                        size: size
                );
            return list;
        }

        public async Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawing(Guid id)
        {
            var drawingItem = await _unitOfWork.GetRepository<HouseDesignDrawing>().FirstOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.HouseDesignVersions)
                               .ThenInclude(x => x.Media)
                               .Include(x => x.Account!)
                               .Include(x => x.Project)
            );

            if (drawingItem == null)
            {
                throw new MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.House_Design_Not_Found);
            }

            
            var initialQuotationFinalized = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                            predicate: i => i.ProjectId == drawingItem.ProjectId && i.Status == AppConstant.QuotationStatus.FINALIZED);

            if (drawingItem.Project.Type == AppConstant.Type.DRAWINGHAVE)
            {
                initialQuotationFinalized = null;
            }

            List<DependOnVersion> dependOnVersions = new();

            if (drawingItem.Step > 1)
            {
                // Lấy tất cả các bước trước (1 -> step-1) với trạng thái "Accepted"
                var previousDrawings = await _unitOfWork.GetRepository<HouseDesignDrawing>().GetListAsync(
                    predicate: d => d.ProjectId == drawingItem.ProjectId &&
                                    d.Step < drawingItem.Step &&
                                    d.Status == AppConstant.HouseDesignStatus.ACCEPTED,
                    include: d => d.Include(d => d.HouseDesignVersions)
                                   .ThenInclude(d => d.Media)
                );

                // Lấy các phiên bản chấp nhận được của các bản vẽ trước
                foreach (var previousDrawing in previousDrawings.OrderBy(d => d.Step))
                {
                    var acceptedVersion = previousDrawing.HouseDesignVersions
                                                         .Where(d => d.Confirmed == true)         
                                                         .OrderByDescending(v => v.Version!).FirstOrDefault();

                    if (acceptedVersion != null)
                    {
                        var previousFileUrl = acceptedVersion.Media?.FirstOrDefault(m => m.HouseDesignVersionId == 
                        acceptedVersion.Id)?.Url ?? "Chưa hoàn thành";

                        dependOnVersions.Add(new DependOnVersion
                        {
                            HouseDesginVersionId = acceptedVersion.Id,
                            HouseDesignVersionName = acceptedVersion.Name,
                            HouseDesignVersion = acceptedVersion.Version,
                            FileDesignVersion = previousFileUrl
                        });
                    }
                }
            }

            var result = new HouseDesignDrawingResponse(
                projectType: drawingItem.Project.Type,
                initialQuotationId: initialQuotationFinalized != null ? initialQuotationFinalized.Id : Guid.Empty,
                id: drawingItem.Id,
                projectId: drawingItem.ProjectId,
                staffName: drawingItem.Account.Username,
                versionPresent: drawingItem.HouseDesignVersions.Any()
                    ? drawingItem.HouseDesignVersions.Max(v => v.Version)
                    : 0.0,
                name: drawingItem.Name,
                step: drawingItem.Step,
                status: drawingItem.Status,
                type: drawingItem.Type,
                isCompany: drawingItem.HaveDrawing,
                insDate: drawingItem.InsDate,
                dependOnVersion: dependOnVersions,
                versions: drawingItem.HouseDesignVersions.Select(version => new HouseDesignVersionResponse(
                    id: version.Id,
                    name: version.Name,
                    version: version.Version,
                    fileUrl: version.Media.Select(m => m.Url).FirstOrDefault(),
                    insDate: version.InsDate,
                    previousDrawingId: version.PreviousDrawingId,
                    note: version.Note,
                    reason: version.Reason
                )).OrderByDescending(v => v.Version).ToList()
            );

            return result;
        }

        public async Task<HouseDesignDrawingResponse> GetDetailHouseDesignDrawingByType(string type)
        {
            var drawingItem = await _unitOfWork.GetRepository<HouseDesignDrawing>().FirstOrDefaultAsync(
                predicate: x => x.Type!.Equals(type),
                include: x => x.Include(x => x.HouseDesignVersions)
                                .ThenInclude(x => x.Media)
                                .Include(x => x.Account!)
                               .Include(x => x.Project)
            );

            if (drawingItem == null)
            {
                throw new InvalidOperationException($"No drawing found for type: {type}");
            }

            var initialQuotationFinalized = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(
                                           predicate: i => i.ProjectId == drawingItem.ProjectId && i.Status == AppConstant.QuotationStatus.FINALIZED);

            List<DependOnVersion> dependOnVersions = new();

            if (drawingItem.Step > 1)
            {
                // Lấy tất cả các bước trước (1 -> step-1) với trạng thái "Accepted"
                var previousDrawings = await _unitOfWork.GetRepository<HouseDesignDrawing>().GetListAsync(
                    predicate: d => d.ProjectId == drawingItem.ProjectId &&
                                    d.Step < drawingItem.Step &&
                                    d.Status == AppConstant.HouseDesignStatus.ACCEPTED,
                    include: d => d.Include(d => d.HouseDesignVersions)
                                   .ThenInclude(d => d.Media)
                );

                // Lấy các phiên bản chấp nhận được của các bản vẽ trước
                foreach (var previousDrawing in previousDrawings)
                {
                    var acceptedVersion = previousDrawing.HouseDesignVersions
                                                         .OrderByDescending(v => v.Version)
                                                         .FirstOrDefault();

                    if (acceptedVersion != null)
                    {
                        var previousFileUrl = acceptedVersion.Media?.FirstOrDefault(m => m.HouseDesignVersionId == acceptedVersion.Id)?.Url ?? "Chưa hoàn thành";
                        dependOnVersions.Add(new DependOnVersion
                        {
                            HouseDesginVersionId = acceptedVersion.Id,
                            HouseDesignVersionName = acceptedVersion.Name,
                            HouseDesignVersion = acceptedVersion.Version,
                            FileDesignVersion = previousFileUrl
                        });
                    }
                }
            }

            var result = new HouseDesignDrawingResponse(
                projectType: drawingItem.Project.Type,
                initialQuotationId: initialQuotationFinalized.Id,
                id: drawingItem.Id,
                projectId: drawingItem.ProjectId,
                staffName: drawingItem.Account.Username,
                versionPresent: drawingItem.HouseDesignVersions.Any()
                    ? drawingItem.HouseDesignVersions.Max(v => v.Version)
                    : 0.0,
                name: drawingItem.Name,
                step: drawingItem.Step,
                status: drawingItem.Status,
                type: drawingItem.Type,
                isCompany: drawingItem.HaveDrawing,
                insDate: drawingItem.InsDate,
                dependOnVersion: dependOnVersions,
                versions: drawingItem.HouseDesignVersions.Select(version => new HouseDesignVersionResponse(
                    id: version.Id,
                    name: version.Name,
                    version: version.Version,
                    fileUrl: version.Media.Select(m => m.Url).FirstOrDefault(),
                    insDate: version.InsDate,
                    previousDrawingId: version.PreviousDrawingId,
                    note: version.Note,
                    reason: version.Reason
                )).OrderByDescending(v => v.Version).ToList()
            );

            return result;
        }

        public async Task<(bool IsSuccess, string Message)> CreateListTaskHouseDesignDrawing(HouseDesignDrawingRequest item)
        {
            int stepDrawing = 1;
            var statusDrawing = "Pending";
            Guid designerId = Guid.Empty;

            var projectCheck = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                            predicate: p => p.Id == item.ProjectId,
                            include: p => p.Include(p => p.InitialQuotations)
                                            .Include(p => p.Contracts));
            //Check project
            if (projectCheck == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.ProjectNotExit);
            }

            //Check initial quotation finalized ?
            if (!projectCheck.InitialQuotations.Any(i => i.Status == AppConstant.QuotationStatus.FINALIZED))
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.NotFinalizedQuotationInitial);
            }
            //Check contract desgin finished ?
            if (projectCheck.Contracts != null &&
            !projectCheck.Contracts.Any(c => c.Type == ContractType.Design.ToString()))
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Unprocessable_Entity, AppConstant.ErrMessage.NotStartContractDesign);
            }

            //Check number of drawing
            int existingDrawingsCount = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                                                     .CountAsync(d => d.ProjectId == item.ProjectId);

            //Check request - design staff not 4 design task
            var isValid = await ValidateHouseDesignDrawingRequest(item);

            if (!isValid)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.RequestOverloadStaff
                );
            }

            if (existingDrawingsCount >= 4)
            {
                return (false, AppConstant.ErrMessage.OverloadProjectDrawing);
            }

            foreach (DesignDrawing designType in Enum.GetValues(typeof(DesignDrawing)))
            {
                switch (designType)
                {
                    case DesignDrawing.Perspective:
                        statusDrawing = "Processing";
                        designerId = item.DesignerPerspective;
                        break;

                    case DesignDrawing.Architecture:
                        statusDrawing = "Pending";
                        designerId = item.DesignerArchitecture;
                        break;

                    case DesignDrawing.Structure:
                        statusDrawing = "Pending";
                        designerId = item.DesignerStructure;
                        break;

                    case DesignDrawing.ElectricityWater:
                        statusDrawing = "Pending";
                        designerId = item.DesignerElectricityWater;
                        break;
                }

                int existingDesigns = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                    .CountAsync(d => d.AccountId == designerId && d.Status != AppConstant.HouseDesignStatus.ACCEPTED
                    && d.Status != AppConstant.HouseDesignStatus.FINALIZED
                    && d.Status != AppConstant.HouseDesignStatus.REJECTED);

                if (existingDesigns >= 2)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.OverloadStaff);
                }


                var houseDrawing = new HouseDesignDrawing
                {
                    Id = Guid.NewGuid(),
                    ProjectId = item.ProjectId,
                    Name = designType.GetEnumDescription(),
                    Step = stepDrawing++,
                    Status = statusDrawing,
                    Type = designType.ToTypeString(),
                    HaveDrawing = false,
                    InsDate = LocalDateTime.VNDateTime(),
                    AccountId = designerId
                };

                await _unitOfWork.GetRepository<HouseDesignDrawing>().InsertAsync(houseDrawing);
            }


            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful ? (true, AppConstant.Message.APPROVED) : (false, "Error occurred during saving.");
        }

        public async Task<bool> ValidateHouseDesignDrawingRequest(HouseDesignDrawingRequest request)
        {
            var designers = new List<Guid>
            {
                request.DesignerPerspective,
                request.DesignerArchitecture,
                request.DesignerStructure,
                request.DesignerElectricityWater
            };

            var designerTaskCounts = designers
                .GroupBy(d => d)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var designer in designerTaskCounts)
            {
                var designerId = designer.Key;
                var taskCountInRequest = designer.Value;

                var existingTaskCount = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                    .CountAsync(d => d.AccountId == designerId
                        && d.Status != AppConstant.HouseDesignStatus.ACCEPTED
                        && d.Status != AppConstant.HouseDesignStatus.FINALIZED
                        && d.Status != AppConstant.HouseDesignStatus.REJECTED);

                var totalTaskCount = existingTaskCount + taskCountInRequest;

                if (totalTaskCount > 2)
                {
                    return false; 
                }
            }

            return true; 
        }

        public async Task<List<ListHouseDesginResponse>> GetListTaskByAccount(Guid accountId)
        {
            var listTask = (await _unitOfWork.GetRepository<HouseDesignDrawing>().GetList(
                predicate: x => x.Account!.Id == accountId,
                selector: x => new ListHouseDesginResponse(x.Id, x.ProjectId,
                                                          x.Account!.Username,
                                                          x.Name, x.Step, x.Status,
                                                          x.Type, x.HaveDrawing, x.InsDate,
                                                          x.HouseDesignVersions.Select(
                                                              v => new HouseDesignVersionResponseList(
                                                                  v.Id,
                                                                  v.Name,
                                                                  v.Version,
                                                                  v.Media.Select(m => m.Url).FirstOrDefault(),
                                                                  v.InsDate,
                                                                  v.PreviousDrawingId,
                                                                  v.Note,
                                                                  v.Reason, 
                                                                  v.Confirmed)).ToList()),
                include: x => x.Include(x => x.Account!)
                                .Include(x => x.HouseDesignVersions)
                                   .ThenInclude(x => x.Media)
            )).Items.ToList();
            return listTask;
        }

        public async Task<List<ListHouseDesginResponse>> ViewDrawingPreviousStep(Guid accountId, Guid projectId)
        {
            //Check design staff in step?
            var infoDesign = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                                        .FirstOrDefaultAsync(predicate: x => x.Account!.Id == accountId && x.ProjectId == projectId,
                                        include: x => x.Include(x => x.HouseDesignVersions));
            if (infoDesign is null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.DesignNoAccess);
            //Case Drawing "Phối cảnh"
            if (infoDesign.Step == 1)
            {
                return new List<ListHouseDesginResponse>();
            }
            //Case other...
            else
            {
                var listDrawingPrevious = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                                                .GetList(predicate: x => x.ProjectId == projectId && x.Step < infoDesign.Step,
                                                 selector: x => new ListHouseDesginResponse(x.Id, x.ProjectId,
                                                          x.Account.Username!,
                                                          x.Name, x.Step, x.Status,
                                                          x.Type, x.HaveDrawing, x.InsDate,
                                                          x.HouseDesignVersions.Select(
                                                              v => new HouseDesignVersionResponseList(
                                                                  v.Id,
                                                                  v.Name,
                                                                  v.Version,
                                                                  v.Media.Select(m => m.Url).FirstOrDefault(),
                                                                  v.InsDate,
                                                                  v.PreviousDrawingId,
                                                                  v.Note,
                                                                  v.Reason, 
                                                                  v.Confirmed)).ToList()),
                        include: x => x.Include(x => x.HouseDesignVersions)
                                        .ThenInclude(x => x.Media)
                                        .Include(x => x.Account!),
                        orderBy: x => x.OrderBy(x => x.Step));
                return listDrawingPrevious.Items.ToList();

            }
        }

        public async Task<List<ListHouseDesginResponse>> ViewDrawingByProjectId(Guid projectId)
        {
            var listDrawingPrevious = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                                                .GetList(predicate: x => x.ProjectId == projectId &&
                                                         (x.Status == AppConstant.HouseDesignStatus.APPROVED ||
                                                         x.Status == AppConstant.HouseDesignStatus.UPDATING ||
                                                         x.Status == AppConstant.HouseDesignStatus.ACCEPTED ||
                                                         x.Status == AppConstant.HouseDesignStatus.FINALIZED),
                                                 selector: x => new ListHouseDesginResponse(x.Id, x.ProjectId,
                                                          x.Account.Username!,
                                                          x.Name, x.Step, x.Status,
                                                          x.Type, x.HaveDrawing, x.InsDate,
                                                          x.HouseDesignVersions
                                                          .Select(
                                                              v => new HouseDesignVersionResponseList(
                                                                  v.Id,
                                                                  v.Name,
                                                                  v.Version,
                                                                  v.Media.Select(m => m.Url).FirstOrDefault(),
                                                                  v.InsDate,
                                                                  v.PreviousDrawingId,
                                                                  v.Note,
                                                                  v.Reason,
                                                                  v.Confirmed)).ToList()),
                        include: x => x.Include(x => x.HouseDesignVersions)
                                        .ThenInclude(x => x.Media)
                                        .Include(x => x.Account!),
                        orderBy: x => x.OrderBy(x => x.Step));
            return listDrawingPrevious.Items.ToList();
        }

        public async Task<string> CreateProjectHaveDrawing(Guid projectId, Guid accountId, ProjectHaveDrawingRequest files)
        {
            async Task AddDrawingAndVersions(string name, int step, string type, IEnumerable<IFormFile> images)
            {
                var houseDrawing = new HouseDesignDrawing
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    Name = name,
                    Step = step,
                    Status = AppConstant.HouseDesignStatus.REVIEWING,
                    Type = type,
                    HaveDrawing = true,
                    InsDate = LocalDateTime.VNDateTime(),
                    AccountId = accountId
                };

                await _unitOfWork.GetRepository<HouseDesignDrawing>().InsertAsync(houseDrawing);

                foreach (var imageFile in images)
                {
                    var imageUrl = await _mediaService.UploadFileAsync(imageFile, "DrawingHave");
                    if (imageUrl != null)
                    {
                        var houseDesignVersion = new HouseDesignVersion
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            Version = 0.0,
                            InsDate = LocalDateTime.VNDateTime(),
                            HouseDesignDrawingId = houseDrawing.Id,
                            UpsDate = LocalDateTime.VNDateTime(),
                            RelatedDrawingId = null,
                            PreviousDrawingId = null,
                            Reason = null,
                            Deflag = true
                        };

                        await _unitOfWork.GetRepository<HouseDesignVersion>().InsertAsync(houseDesignVersion);

                        var media = new Medium
                        {
                            Id = Guid.NewGuid(),
                            HouseDesignVersionId = houseDesignVersion.Id,
                            Name = AppConstant.Template.Drawing,
                            Url = imageUrl,
                            InsDate = LocalDateTime.VNDateTime(),
                            UpsDate = LocalDateTime.VNDateTime()
                        };

                        await _unitOfWork.GetRepository<Medium>().InsertAsync(media);
                    }
                }
            }

            await AddDrawingAndVersions(
                DesignDrawingExtensions.ToTypeString(AppConstant.DesignDrawing.Perspective),
                1,
                AppConstant.Type.PHOICANH,
                files.PerspectiveImage
            );

            await AddDrawingAndVersions(
                DesignDrawingExtensions.ToTypeString(AppConstant.DesignDrawing.Architecture),
                2,
                AppConstant.Type.KIENTRUC,
                files.ArchitectureImage
            );

            await AddDrawingAndVersions(
                DesignDrawingExtensions.ToTypeString(AppConstant.DesignDrawing.Structure),
                3,
                AppConstant.Type.KETCAU,
                files.StructureImage
            );

            await AddDrawingAndVersions(
                DesignDrawingExtensions.ToTypeString(AppConstant.DesignDrawing.ElectricityWater),
                4,
                AppConstant.Type.DIENNUOC,
                files.ElectricityWaterImage
            );

            var result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;
            return result;
        }

        public async Task<string> ConfirmDrawingAvaliable(Guid versionId, AssignHouseDrawingRequest request)
        {

            #region Confirm 4 drawing
            var drawingItem = await _unitOfWork.GetRepository<HouseDesignVersion>().FirstOrDefaultAsync(x => x.Id == versionId,
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
                drawingItem.HouseDesignDrawing.Status = AppConstant.HouseDesignStatus.REJECTED;
                drawingItem.Reason = request.Reason;
            }
            #endregion

            #region Check status project
            if (drawingItem.HouseDesignDrawing.Step == 4)
            {
                var projectDrawings = await _unitOfWork.GetRepository<HouseDesignDrawing>()
                    .GetListAsync(predicate: x => x.ProjectId == drawingItem.HouseDesignDrawing.ProjectId && x.Step == 4);

                bool allApproved = projectDrawings.All(d => d.Status == AppConstant.HouseDesignStatus.APPROVED);

                if (allApproved)
                {
                    var projectInfo = await _unitOfWork.GetRepository<Project>()
                        .FirstOrDefaultAsync(x => x.Id == drawingItem.HouseDesignDrawing.ProjectId);

                    if (projectInfo != null)
                    {
                        projectInfo.Status = AppConstant.ProjectStatus.SIGNED_CONTRACT;
                        _unitOfWork.GetRepository<Project>().UpdateAsync(projectInfo);
                    }
                }
                else
                {
                    var projectInfo = await _unitOfWork.GetRepository<Project>()
                        .FirstOrDefaultAsync(x => x.Id == drawingItem.HouseDesignDrawing.ProjectId);

                    if (projectInfo != null && projectInfo.Status != AppConstant.ProjectStatus.PROCESSING)
                    {
                        projectInfo.Status = AppConstant.ProjectStatus.PROCESSING;
                        _unitOfWork.GetRepository<Project>().UpdateAsync(projectInfo);
                    }
                }
            }
            #endregion

            _unitOfWork.GetRepository<HouseDesignVersion>().UpdateAsync(drawingItem);

            var result = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_UPDATE : AppConstant.ErrMessage.Send_Fail;
            return result;
        }

        public async Task<string> DesignRequirements(Guid projectId)
        {
            var projectInfo = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                                predicate: p => p.Id == projectId,
                                include: p => p.Include(p => p.Customer!));

            if (projectInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.ProjectNotExit);
            }

            projectInfo.IsContractDesign = true;

            _unitOfWork.GetRepository<Project>().UpdateAsync(projectInfo);

            var isSuccessful = _unitOfWork.Commit() > 0 ? AppConstant.Message.SEND_SUCESSFUL : AppConstant.ErrMessage.Send_Fail;
            return isSuccessful;
        }
        public async Task<string> GetStatusHouseDesign(Guid houseDesignId)
        {
            var designInfo = await _unitOfWork.GetRepository<HouseDesignDrawing>().FirstOrDefaultAsync(predicate: i => i.Id == houseDesignId);
            if (designInfo == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.House_Design_Not_Found);
            }
            string result = designInfo.Status;
            return result;
        }
    }
}
