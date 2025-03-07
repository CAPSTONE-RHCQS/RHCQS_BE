﻿using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Request.Project;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.Project;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_Services.Implement
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProjectService> _logger;
        private readonly IHouseDesignDrawingService _drawingService;
        public IAuthService _authService { get; private set; }

        public ProjectService(IUnitOfWork unitOfWork,
            ILogger<ProjectService> logger,
            IAuthService authService,
            IHouseDesignDrawingService drawingService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _authService = authService;
            _drawingService = drawingService;
        }

        public async Task<IPaginate<ProjectResponse>> GetProjects(int page, int size)
        {
            IPaginate<ProjectResponse> listProjects =
            await _unitOfWork.GetRepository<Project>().GetList(
                selector: x => new ProjectResponse(x.Id, x.CustomerName!, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer!),
                orderBy: x => x.OrderByDescending(w => w.InsDate),
                page: page,
                size: size
                );
            return listProjects;
        }

        public async Task<IPaginate<ProjectResponse>> FilterProjects(int page, int size, string type)
        {
            IPaginate<ProjectResponse> listProjects =
            await _unitOfWork.GetRepository<Project>().GetList(
                predicate: x => x.Status!.ToUpper() == type.ToUpper(),
                selector: x => new ProjectResponse(x.Id, x.CustomerName!, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer!),
                orderBy: x => x.OrderBy(w => w.InsDate),
                page: page,
                size: size
                );
            return listProjects;
        }

        public async Task<IPaginate<ProjectResponse>> GetListProjectBySalesStaff(Guid accountId, int page, int size)
        {
            IPaginate<ProjectResponse> paginatedProjects = await _unitOfWork.GetRepository<AssignTask>().GetList(
                predicate: x => x.AccountId == accountId,
                selector: x => new ProjectResponse(x.Project!.Id, x.Project.CustomerName!, x.Project.Name, x.Project.Type,
                                                    x.Project.Status, x.Project.InsDate, x.Project.UpsDate, x.Project.ProjectCode),
                include: x => x.Include(x => x.Project!)
                                .ThenInclude(x => x.Customer!),
                orderBy: x => x.OrderByDescending(x => x.Project!.Status == AppConstant.ProjectStatus.PROCESSING)
                               .ThenBy(x => x.InsDate),
                page: page,
                size: size
            );


            List<ProjectResponse> lists = paginatedProjects.Items.ToList();

            return paginatedProjects;
        }

        public async Task<IPaginate<ProjectResponse>> SearchProjectByName(string name, int page, int size)
        {
            string normalizedName = name.ToUpper();
            IPaginate<ProjectResponse> listProjects =
            await _unitOfWork.GetRepository<Project>().GetList(
                //predicate: x => x.Name.ToUpper() == name.ToUpper(),
                predicate: x => x.Name.ToUpper().StartsWith(normalizedName),
                selector: x => new ProjectResponse(x.Id, x.CustomerName!, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer!),
                orderBy: x => x.OrderBy(w => w.InsDate),
                page: page,
                size: size
                );
            if (listProjects.Items.Count == 0)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.ProjectNotExit);
            }
            return listProjects;
        }

        public async Task<ProjectDetail> GetDetailProjectById(Guid id)
        {
            var projectItem = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(w => w.Id == id,
                                include: w => w.Include(p => p.Customer)
                                                .Include(p => p.AssignTasks)
                                                .ThenInclude(p => p.Account)
                                                .Include(p => p.InitialQuotations)
                                                .Include(p => p.FinalQuotations)
                                                .Include(p => p.HouseDesignDrawings)
                                                    .ThenInclude(p => p.Account)
                                                .Include(p => p.HouseDesignDrawings)
                                                    .ThenInclude(h => h.HouseDesignVersions)
                                                .Include(p => p.HouseDesignDrawings)
                                                .Include(p => p.Contracts));
            if (projectItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);

            var initialItem = projectItem.InitialQuotations?
                                        .Where(i => i.Version > 0)
                                        .Select(i => new InitialInfo
                                        {
                                            Id = i.Id,
                                            AccountName = projectItem.AssignTasks.FirstOrDefault()?.Account.Username,
                                            Version = i.Version,
                                            InsDate = i.InsDate,
                                            Status = i.Status
                                        }).OrderByDescending(i => i.InsDate)
                                        .ToList() ?? new List<InitialInfo>();

            var finalItem = projectItem.FinalQuotations?
                                .Where(d => d.Version > 0)
                              .Select(d => new FinalInfo
                              {
                                  Id = d.Id,
                                  AccountName = projectItem.AssignTasks.FirstOrDefault()?.Account.Username,
                                  Version = d.Version,
                                  InsDate = d.InsDate,
                                  Status = d.Status
                              }).OrderByDescending(d => d.InsDate)
                              .ToList() ?? new List<FinalInfo>();


            var houseDesignItem = projectItem.HouseDesignDrawings?
                                                                     .Select(h => new HouseDesignDrawingInfo
                                                                     {
                                                                         Id = h.Id,
                                                                         DesignName = h.Account!.Username!,
                                                                         Step = h.Step,
                                                                         Name = h.Name!,
                                                                         Type = h.Type!,
                                                                         InsDate = h.InsDate,
                                                                         Status = h.Status
                                                                     }).OrderBy(h => h.Step).ToList() ?? new List<HouseDesignDrawingInfo>();
            bool isCustomerUpload = projectItem.HouseDesignDrawings.Any() &&
                        projectItem.HouseDesignDrawings.All(h => h.IsCustomerUpload == true);

            var contractItem = projectItem.Contracts?.Select(c => new ContractInfo
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status,
                Note = c.Note,
                FileContract = c.UrlFile ?? null
            })
            .ToList() ?? new List<ContractInfo>();

            var finalizedInitial = projectItem.InitialQuotations?.FirstOrDefault(i => i.Status == AppConstant.ProjectStatus.FINALIZED);
            var area = finalizedInitial?.Area ?? 0;

            var projectDetailItem = new ProjectDetail
            {
                Id = projectItem.Id,
                CustomerId = (Guid)projectItem.Customer.AccountId!,
                Name = projectItem.Name,
                Phone = projectItem.Customer.PhoneNumber ?? "Không có",
                Avatar = projectItem.Customer.ImgUrl,
                AccountName = projectItem.CustomerName!,
                Address = projectItem.Address,
                Mail = projectItem.Customer!.Email!,
                Area = area == 0 ? projectItem.Area : area,
                Type = projectItem.Type,
                Status = projectItem.Status,
                InsDate = projectItem.InsDate,
                UpsDate = projectItem.UpsDate,
                ProjectCode = projectItem.ProjectCode,
                StaffName = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.Username)),
                StaffPhone = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.PhoneNumber)),
                StaffAvatar = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.ImageUrl)),
                IsDrawing = projectItem.IsDrawing,
                ReasonCanceled = projectItem.ReasonCanceled,
                IsCustomerUpload = isCustomerUpload,
                InitialInfo = initialItem,
                HouseDesignDrawingInfo = houseDesignItem,
                FinalInfo = finalItem,
                ContractInfo = contractItem,
            };

            return projectDetailItem;
        }

        public async Task<ProjectDesignStaffResponse> GetDetailProjectByIdForDesignStaff(Guid id, Guid accountId)
        {
            var projectItem = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(w => w.Id == id
                                && w.HouseDesignDrawings.Any(h => h.AccountId == accountId),
                                include: w => w.Include(p => p.Customer)
                                                .Include(p => p.AssignTasks)
                                                .ThenInclude(p => p.Account)
                                                .Include(p => p.InitialQuotations)
                                                .Include(p => p.HouseDesignDrawings)
                                                    .ThenInclude(p => p.Account)
                                                .Include(p => p.HouseDesignDrawings)
                                                    .ThenInclude(h => h.HouseDesignVersions)
                                                .Include(p => p.HouseDesignDrawings));
            if (projectItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);

            var initialItem = projectItem.InitialQuotations?
                                        .Where(i => i.Version > 0 && i.Status == AppConstant.QuotationStatus.FINALIZED)
                                        .Select(i => new InitialInfo
                                        {
                                            Id = i.Id,
                                            AccountName = projectItem.AssignTasks.FirstOrDefault()?.Account.Username,
                                            Version = i.Version,
                                            InsDate = i.InsDate,
                                            Status = i.Status
                                        }).OrderByDescending(i => i.InsDate)
                                        .ToList() ?? new List<InitialInfo>();

            var finalItem = projectItem.FinalQuotations?
                                .Where(d => d.Version > 0)
                              .Select(d => new FinalInfo
                              {
                                  Id = d.Id,
                                  AccountName = projectItem.AssignTasks.FirstOrDefault()?.Account.Username,
                                  Version = d.Version,
                                  InsDate = d.InsDate,
                                  Status = d.Status
                              }).OrderByDescending(d => d.InsDate)
                              .ToList() ?? new List<FinalInfo>();


            var currentStep = projectItem.HouseDesignDrawings
                              .FirstOrDefault(h => h.AccountId == accountId)?.Step;

            var houseDesignItem = projectItem.HouseDesignDrawings?
                .Where(h => h.Step < currentStep && h.Status == AppConstant.HouseDesignStatus.ACCEPTED)
                .Select(h => new HouseDesignDrawingInfo
                {
                    Id = h.Id,
                    DesignName = h.Account!.Username!,
                    Step = h.Step,
                    Name = h.Name!,
                    Type = h.Type!,
                    InsDate = h.InsDate,
                    Status = h.Status
                }).OrderBy(h => h.Step).ToList() ?? new List<HouseDesignDrawingInfo>();


            var projectDetailItem = new ProjectDesignStaffResponse
            {
                Id = projectItem.Id,
                Name = projectItem.Name,
                Phone = projectItem.Customer.PhoneNumber ?? "Không có",
                Avatar = projectItem.Customer.ImgUrl,
                AccountName = projectItem.CustomerName!,
                Address = projectItem.Address,
                Mail = projectItem.Customer!.Email!,
                Area = projectItem.Area,
                Type = projectItem.Type,
                Status = projectItem.Status,
                InsDate = projectItem.InsDate,
                UpsDate = projectItem.UpsDate,
                ProjectCode = projectItem.ProjectCode,
                StaffName = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.Username)),
                StaffPhone = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.PhoneNumber)),
                StaffAvatar = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.ImageUrl)),
                IsDrawing = projectItem.IsDrawing,
                InitialInfo = initialItem,
                HouseDesignDrawingInfo = houseDesignItem
            };

            return projectDetailItem;
        }

        public async Task<List<ProjectResponse>> GetListProjectByEmail(string email)
        {
            var listProject = await _unitOfWork.GetRepository<Project>()
                .GetList(
                    predicate: p => p.Customer!.Email == email,
                    selector: p => new ProjectResponse(
                        p.Id,
                        p.Customer!.Username!,
                        p.Name,
                        p.Type,
                        p.Status,
                        p.InsDate,
                        p.UpsDate,
                        p.ProjectCode
                    ),
                    include: p => p.Include(p => p.Customer!)
                );

            return listProject.Items.ToList();
        }

        public async Task<List<ProjectResponse>> SearchProjectByPhone(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.PhoneIsEmpty);
            }

            var projectPaginate = await _unitOfWork.GetRepository<Project>().GetList(
                selector: x => new ProjectResponse(x.Id, x.Customer!.Username!, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer!),
                predicate: x => x.Customer!.PhoneNumber == phoneNumber,
                orderBy: x => x.OrderBy(w => w.InsDate));

            return projectPaginate.Items.ToList();
        }

        public async Task<bool> CreateProjectQuotation(ProjectRequest projectRequest)
        {
            try
            {
                var customerInfo = await _unitOfWork.GetRepository<Customer>().FirstOrDefaultAsync(x => x.AccountId == projectRequest.CustomerId);
                if (customerInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invalid_Customer);
                }

                if (projectRequest.Promotion != null)
                {
                    var promotionInfo = await _unitOfWork.GetRepository<Promotion>().FirstOrDefaultAsync(
                                    predicate: p => p.Id == projectRequest.Promotion.Id && p.ExpTime >= LocalDateTime.VNDateTime() && p.IsRunning == true,
                                    include: p => p.Include(p => p.PackageMapPromotions)
                                                    .ThenInclude(p => p.Package));

                    if (promotionInfo == null)
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, AppConstant.ErrMessage.PromotionIllegal);
                    }

                    if (!projectRequest.PackageQuotations
                        .Any(package => promotionInfo.PackageMapPromotions
                            .Any(p => p.PackageId == package.PackageId)))
                    {
                        throw new AppConstant.MessageError((int)AppConstant.ErrCode.Conflict, AppConstant.ErrMessage.PromotionIllegal);
                    }
                }
                string projectName;
                switch (projectRequest.Type)
                {
                    case AppConstant.Type.ALL:
                        projectName = "Dự án báo giá trọn gói ";
                        break;
                    case AppConstant.Type.FINISHED:
                        projectName = "Dự án báo giá phần hoàn thiện ";
                        break;
                    case AppConstant.Type.ROUGH:
                        projectName = "Dự án báo giá phần thô ";
                        break;
                    default:
                        projectName = "Dự án báo giá mẫu nhà ";
                        break;
                }

                #region Create project
                Project projectItem = null;
                //Case khách hàng có 4 bản vẽ: IsDrawing = true
                if (projectRequest.IsDrawing == true)
                {
                    projectItem = new Project
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customerInfo.Id,
                        Name = projectName,
                        Type = AppConstant.Type.DRAWINGHAVE,
                        Status = AppConstant.ProjectStatus.PROCESSING,
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        ProjectCode = GenerateRandom.GenerateRandomString(5),
                        Address = projectRequest.Address,
                        Area = projectRequest.Area,
                        IsDrawing = true,
                        CustomerName = projectRequest.CustomerName
                    };
                }
                else
                {
                    projectItem = new Project
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customerInfo.Id,
                        Name = projectName,
                        Type = projectRequest.Type,
                        Status = AppConstant.ProjectStatus.PROCESSING,
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        ProjectCode = GenerateRandom.GenerateRandomString(5),
                        Address = projectRequest.Address,
                        Area = projectRequest.Area,
                        IsDrawing = false,
                        CustomerName = projectRequest.CustomerName
                    };
                }

                await _unitOfWork.GetRepository<Project>().InsertAsync(projectItem);
                #endregion

                #region Initial quotation
                var initialItem = new InitialQuotation
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectItem.Id,
                    PromotionId = projectRequest.Promotion?.Id,
                    Area = projectRequest.Area,
                    TimeProcessing = null,
                    TimeRough = null,
                    TimeOthers = null,
                    InsDate = LocalDateTime.VNDateTime(),
                    Status = AppConstant.QuotationStatus.PENDING,
                    Version = 0.0,
                    IsTemplate = false,
                    Deflag = false,
                    Note = null,
                    ReasonReject = null,
                    IsDraft = false
                };

                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);
                #endregion

                #region PackageQuotation
                if (projectRequest.PackageQuotations.Count < 1)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.NotFound,
                        AppConstant.ErrMessage.InvalidPackageQuotation);
                }
                foreach (var package in projectRequest.PackageQuotations)
                {
                    var packageQuotation = new PackageQuotation
                    {
                        Id = Guid.NewGuid(),
                        PackageId = package.PackageId,
                        InitialQuotationId = initialItem.Id,
                        Type = package.Type,
                        InsDate = LocalDateTime.VNDateTime()
                    };

                    await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageQuotation);
                }
                #endregion

                #region Create initial quotation item
                foreach (var request in projectRequest.InitialQuotation.InitialQuotationItemRequests!)
                {
                    var initialQuotationItem = new InitialQuotationItem
                    {
                        Id = Guid.NewGuid(),
                        Name = "",
                        ConstructionItemId = request.ConstructionItemId,
                        SubConstructionId = request.SubConstructionId,
                        Area = request.Area,
                        Price = request.Price,
                        UnitPrice = "đ",
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        InitialQuotationId = initialItem.Id
                    };
                    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(initialQuotationItem);
                }
                #endregion

                #region Create utility
                if (projectRequest.QuotationUtilitiesRequest != null)
                {
                    foreach (var utl in projectRequest.QuotationUtilitiesRequest!)
                    {
                        var utilityItem = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(u => u.Id == utl.UtilitiesItemId);
                        Guid? sectionId = null;
                        QuotationUtility utlItem;
                        //UtilityItem - null => utl.UtilitiesItem = SectionId
                        //UtilityItem != null => utl.UltilitiesItemId = UtilityItem.Id, SectionId = UltilitiesItemId.SectionId
                        if (utilityItem == null)
                        {
                            sectionId = utl.UtilitiesItemId;
                            var sectionItem = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(u => u.Id == sectionId);
                            utlItem = new QuotationUtility
                            {
                                Id = Guid.NewGuid(),
                                UtilitiesItemId = null,
                                FinalQuotationId = null,
                                InitialQuotationId = initialItem.Id,
                                Name = sectionItem.Name!,
                                Coefficient = 0,
                                Price = utl.Price,
                                Description = sectionItem.Description,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime(),
                                UtilitiesSectionId = sectionItem.Id,
                                Quanity = utl.Quantity ?? 0,
                                TotalPrice = utl.TotalPrice
                            };
                        }
                        else
                        {
                            sectionId = utilityItem.SectionId;
                            utl.UtilitiesItemId = utilityItem.Id;
                            utlItem = new QuotationUtility
                            {
                                Id = Guid.NewGuid(),
                                UtilitiesItemId = utilityItem.Id,
                                FinalQuotationId = null,
                                InitialQuotationId = initialItem.Id,
                                Name = utilityItem.Name!,
                                Coefficient = utilityItem.Coefficient,
                                Price = utl.Price,
                                Description = utilityItem.Name,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime(),
                                UtilitiesSectionId = utilityItem.SectionId,
                                Quanity = utl.Quantity ?? 0,
                                TotalPrice = utl.TotalPrice
                            };
                        }

                        await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
                    }
                }
                #endregion

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
            catch (AppConstant.MessageError ex)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Internal_Server_Error, ex.Message);
            }
        }

        public async Task<string> AssignQuotation(Guid accountId, Guid projectId)
        {
            var projectCount = await _unitOfWork.GetRepository<AssignTask>()
                                .CountAsync(at => at.AccountId == accountId &&
                                    at.Project.Status == AppConstant.ProjectStatus.PROCESSING);

            var projectInfo = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(predicate: p => p.Id == projectId);

            var projectAval = await _unitOfWork.GetRepository<AssignTask>().CountAsync(a => a.ProjectId == projectId);
            if (projectAval >= 1) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Too_Many_Requests, AppConstant.ErrMessage.QuotationHasStaff);
            if (projectCount >= 2)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Too_Many_Requests, AppConstant.ErrMessage.OverloadStaff);
            }

            var assignItem = new AssignTask
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                ProjectId = projectId,
                InsDate = LocalDateTime.VNDateTime()
            };

            //var initialInfo = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.Version == 0.0);

            await _unitOfWork.GetRepository<AssignTask>().InsertAsync(assignItem);

            //Update Initial quotation status PENDING -> PROCESSING
            if (projectInfo.Type != AppConstant.Type.DRAWINGHAVE)
            {
                var initialInfo = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.ProjectId == projectId);
                initialInfo.Status = AppConstant.QuotationStatus.PROCESSING;
                _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialInfo);
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful ? "Phân công Sales thành công!" : throw new Exception("Phân công thất bại!");
        }

        public async Task<bool> CancelProject(Guid projectId, string reasonCanceled)
        {
            var infoProject = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                            predicate: x => x.Id == projectId,
                            include: x => x.Include(x => x.InitialQuotations)
                                           .Include(x => x.FinalQuotations)
                                           .Include(x => x.HouseDesignDrawings)
                                           .Include(x => x.Contracts));
            
            if (infoProject == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);
            }
            else
            {
                try
                {
                    //Cancel Project
                    infoProject.Status = AppConstant.ProjectStatus.ENDED;
                    infoProject.ReasonCanceled = reasonCanceled;
                    //infoProject.ReasonCancle
                    _unitOfWork.GetRepository<Project>().UpdateAsync(infoProject);

                    //Cancel Initial Quotation
                    foreach (var item in infoProject.InitialQuotations)
                    {
                        item.Deflag = false;
                        item.Status = AppConstant.QuotationStatus.CANCELED;
                        _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(item);
                    }

                    //Cancel Final Quotation
                    foreach (var item in infoProject.FinalQuotations)
                    {
                        item.Deflag = false;
                        item.Status = AppConstant.QuotationStatus.CANCELED;
                        _unitOfWork.GetRepository<FinalQuotation>().UpdateAsync(item);
                    }

                    //Cancel HouseDesignDrawing
                    foreach (var item in infoProject.HouseDesignDrawings)
                    {
                        item.Status = AppConstant.HouseDesignStatus.CANCELED;
                        _unitOfWork.GetRepository<HouseDesignDrawing>().UpdateAsync(item);
                    }

                    //Canel Contract
                    foreach (var item in infoProject.Contracts)
                    {
                        item.Status = AppConstant.ContractStatus.ENDED;
                        _unitOfWork.GetRepository<Contract>().UpdateAsync(item);

                        //Cancel batch payment
                        var listBatch = await _unitOfWork.GetRepository<BatchPayment>().GetListAsync(
                                        predicate: bp => bp.ContractId == item.Id);
                        if (listBatch != null && listBatch.Any())
                        {
                            foreach (var batch in listBatch)
                            {
                                batch.Status = "Canceled";
                                _unitOfWork.GetRepository<BatchPayment>().UpdateAsync(batch);
                            }
                        }

                        //Cancel contract appendix
                        var contractAppendices = await _unitOfWork.GetRepository<Contract>()
                                                .GetListAsync(predicate: c => c.ContractAppendix == item.Id);
                        if (contractAppendices != null && contractAppendices.Any())
                        {
                            foreach (var appendix in contractAppendices)
                            {
                                appendix.Status = AppConstant.ContractStatus.ENDED;
                                _unitOfWork.GetRepository<Contract>().UpdateAsync(appendix);

                                var listBatchAppendix = await _unitOfWork.GetRepository<BatchPayment>()
                                                     .GetListAsync(predicate: bp => bp.ContractId == appendix.Id);
                                if (listBatchAppendix != null && listBatchAppendix.Any())
                                {
                                    foreach (var batch in listBatchAppendix)
                                    {
                                        batch.Status = "Canceled";
                                        _unitOfWork.GetRepository<BatchPayment>().UpdateAsync(batch);
                                    }
                                }
                            }
                        }
                    }

                    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    return isSuccessful;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<ProjectAppResponse> TrackingProject(Guid projectId)
        {
            var projectTrack = await _unitOfWork.GetRepository<Project>()
                .FirstOrDefaultAsync(
                    predicate: p => p.Id == projectId,
                    include: p => p.Include(p => p.InitialQuotations)
                                   .Include(p => p.Contracts)
                                   .Include(p => p.FinalQuotations)
                );

            if (projectTrack == null)
            {
                return new ProjectAppResponse(null, null, null, null);
            }

            var initialResponse = await GetInitialQuotationResponse(projectId);
            var finalResponse = await GetFinalQuotationResponse(projectId);
            var processingResponse = await GetContractResponse<ContractProcessingAppResponse>(projectId,
                   AppConstant.ContractType.Construction.ToString());

            //Case: Have drawing - contract design: null
            if (projectTrack.IsDrawing == false || projectTrack.Type == AppConstant.Type.TEMPLATE)
            {
                var contractDesignResponse = await GetContractResponse<ContractDesignAppResponse>(projectId,
                    AppConstant.ContractType.Design.ToString());
                return new ProjectAppResponse(
                    initialResponse,
                    contractDesignResponse,
                    finalResponse,
                    processingResponse
                );
            }
            else
            {
                //Change step Final quotation -> Contract construction
                if (processingResponse == null && finalResponse == null)
                {
                    processingResponse = null;
                    finalResponse = null;
                }
                else if (processingResponse == null && finalResponse.Status == AppConstant.QuotationStatus.FINALIZED)
                {
                    processingResponse = new ContractProcessingAppResponse
                    {
                        Status = AppConstant.ContractStatus.PROCESSING
                    };
                }
            }

            return new ProjectAppResponse(
                initialResponse,
                null,
                finalResponse,
                processingResponse
            );
        }

        #region Get InitialQuotationTracking
        private async Task<InitialAppResponse?> GetInitialQuotationResponse(Guid projectId)
        {
            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>()
                .FirstOrDefaultAsync(
                    predicate: q => q.ProjectId == projectId && q.Status != AppConstant.QuotationStatus.ENDED,
                    orderBy: q => q.OrderByDescending(q => q.Version)
                );

            return initialQuotation != null
                ? new InitialAppResponse { Status = initialQuotation.Status! }
                : null;
        }
        #endregion

        #region Get FinalQuotationTracking
        private async Task<FinalAppResponse?> GetFinalQuotationResponse(Guid projectId)
        {
            var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>()
                .FirstOrDefaultAsync(
                    predicate: q => q.ProjectId == projectId,
                    orderBy: q => q.OrderByDescending(q => q.Version)
                );

            return finalQuotation != null
                ? new FinalAppResponse { Status = finalQuotation.Status! }
                : null;
        }
        #endregion

        #region Support GetContract
        private async Task<T?> GetContractResponse<T>(Guid projectId, string contractType) where T : class
        {
            var contract = await _unitOfWork.GetRepository<Contract>()
                .FirstOrDefaultAsync(
                    predicate: c => c.ProjectId == projectId && c.Type == contractType
                );

            if (contract == null) return null;

            return contractType == AppConstant.ContractType.Design.ToString()
                ? new ContractDesignAppResponse { Status = contract.Status } as T
                : new ContractProcessingAppResponse { Status = contract.Status } as T;
        }
        #endregion

        public async Task<bool> CreateProjectTemplateHouse(TemplateHouseProjectRequest request)
        {
            try
            {
                #region Check template house
                var templateHouseInfo = await _unitOfWork.GetRepository<SubTemplate>().FirstOrDefaultAsync(
                                    predicate: x => x.Id == request.SubTemplateId,
                                    include: x => x.Include(x => x.TemplateItems)
                                                        .ThenInclude(x => x.SubConstruction)
                                                            .ThenInclude(x => x.ConstructionItems)
                                                    .Include(x => x.DesignTemplate)
                                                        .ThenInclude(x => x.PackageHouses)
                                                        .ThenInclude(x => x.Package));
                if (templateHouseInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.TemplateItemNotFound);
                }
                var customerInfo = await _unitOfWork.GetRepository<Customer>().FirstOrDefaultAsync(x => x.AccountId == request.AccountId);
                if (customerInfo == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.Invalid_Customer);
                }
                #endregion

                #region Create project
                var projectItem = new Project
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerInfo.Id,
                    Name = "Dự án báo giá mẫu nhà có sẵn"/*+ LocalDateTime.VNDateTime()*/,
                    Type = AppConstant.Type.TEMPLATE,
                    Status = AppConstant.ProjectStatus.PROCESSING,
                    InsDate = LocalDateTime.VNDateTime(),
                    UpsDate = LocalDateTime.VNDateTime(),
                    ProjectCode = GenerateRandom.GenerateRandomString(5),
                    Address = request.Address,
                    Area = templateHouseInfo.BuildingArea,
                    IsDrawing = false,
                    CustomerName = request.CustomerName ??  customerInfo.Username
                };
                await _unitOfWork.GetRepository<Project>().InsertAsync(projectItem);
                #endregion

                #region Calculate total price rough
                //System find package rough
                var packageRough = await _unitOfWork.GetRepository<Package>().FirstOrDefaultAsync(
                                        predicate: p => p.Type == AppConstant.Type.ROUGH
                                        && p.PackageHouses.Any(p => p.DesignTemplateId == templateHouseInfo.DesignTemplateId));

                //Calculate total rough from buidling area
                double totalPriceRough = 0;
                if (packageRough.Price.HasValue && templateHouseInfo.BuildingArea != 0)
                {
                    totalPriceRough = (double)packageRough.Price * (double)templateHouseInfo.BuildingArea;
                }
                else
                {
                    totalPriceRough = 0;
                }
                #endregion

                #region Create initial quotation
                var initialItem = new InitialQuotation
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectItem.Id,
                    PromotionId = null,
                    Area = templateHouseInfo.BuildingArea,
                    TimeProcessing = null,
                    TimeRough = null,
                    TimeOthers = null,
                    InsDate = LocalDateTime.VNDateTime(),
                    Status = AppConstant.QuotationStatus.PENDING,
                    Version = 0.0,
                    IsTemplate = true,
                    Deflag = false,
                    Note = null,
                    TotalRough = totalPriceRough,
                    ReasonReject = null
                };

                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);
                #endregion

                #region Create package quotation
                var packageRoughQuotation = new PackageQuotation
                {
                    Id = Guid.NewGuid(),
                    PackageId = packageRough.Id,
                    InitialQuotationId = initialItem.Id,
                    Type = AppConstant.Type.ROUGH,
                    InsDate = LocalDateTime.VNDateTime()
                };

                await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageRoughQuotation);

                //Customer choose package finished
                var packageFinishedQuotation = new PackageQuotation
                {
                    Id = Guid.NewGuid(),
                    PackageId = request.PackageFinished,
                    InitialQuotationId = initialItem.Id,
                    Type = AppConstant.Type.FINISHED,
                    InsDate = LocalDateTime.VNDateTime()
                };

                await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageFinishedQuotation);
                #endregion

                #region Create initial quotation item
                foreach (var item in templateHouseInfo.TemplateItems!)
                {
                    var initialQuotationItem = new InitialQuotationItem
                    {
                        Id = Guid.NewGuid(),
                        Name = null,
                        ConstructionItemId = item.ConstructionItemId,
                        SubConstructionId = item.SubConstructionId,
                        Area = item.Area,
                        Price = item.Price,
                        UnitPrice = "đ",
                        InsDate = LocalDateTime.VNDateTime(),
                        UpsDate = LocalDateTime.VNDateTime(),
                        InitialQuotationId = initialItem.Id
                    };
                    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(initialQuotationItem);
                }
                #endregion

                #region Create utility
                if (request.QuotationUtilitiesRequest != null)
                {
                    foreach (var utl in request.QuotationUtilitiesRequest)
                    {
                        var utilityItem = await _unitOfWork.GetRepository<UtilitiesItem>().FirstOrDefaultAsync(u => u.Id == utl.UtilitiesItemId);
                        Guid? sectionId = null;
                        QuotationUtility utlItem;
                        //UtilityItem - null => utl.UtilitiesItem = SectionId
                        //UtilityItem != null => utl.UltilitiesItemId = UtilityItem.Id, SectionId = UltilitiesItemId.SectionId
                        if (utilityItem == null)
                        {
                            sectionId = utl.UtilitiesItemId;
                            var sectionItem = await _unitOfWork.GetRepository<UtilitiesSection>().FirstOrDefaultAsync(u => u.Id == sectionId);
                            utlItem = new QuotationUtility
                            {
                                Id = Guid.NewGuid(),
                                UtilitiesItemId = null,
                                FinalQuotationId = null,
                                InitialQuotationId = initialItem.Id,
                                Name = sectionItem.Name!,
                                Coefficient = 0,
                                Price = utl.Price,
                                Description = sectionItem.Description,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime(),
                                UtilitiesSectionId = sectionItem.Id,
                                Quanity = utl.Quantity ?? 0,
                                TotalPrice = utl.TotalPrice
                            };
                        }
                        else
                        {
                            sectionId = utilityItem.SectionId;
                            utl.UtilitiesItemId = utilityItem.Id;
                            utlItem = new QuotationUtility
                            {
                                Id = Guid.NewGuid(),
                                UtilitiesItemId = utilityItem.Id,
                                FinalQuotationId = null,
                                InitialQuotationId = initialItem.Id,
                                Name = utilityItem.Name!,
                                Coefficient = utilityItem.Coefficient,
                                Price = utl.Price,
                                Description = null,
                                InsDate = LocalDateTime.VNDateTime(),
                                UpsDate = LocalDateTime.VNDateTime(),
                                UtilitiesSectionId = utilityItem.SectionId,
                                Quanity = utl.Quantity ?? 0,
                                TotalPrice = utl.TotalPrice
                            };
                        }

                        await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
                    }
                }
                #endregion

                //Promotion ....
                var saveResutl = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<string> ProjectHaveDrawing(ProjectHaveDrawingRequest request)
        {
            //Check amount of drawing - 4 type drawing
            //1.Perspective - Phối cảnh
            if (request.PerspectiveImage == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, AppConstant.ErrMessage.Invalid_Perspective);
            }

            //2. Architerture - Kiến trúc
            if (request.ArchitectureImage == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, AppConstant.ErrMessage.Invalid_Architecture);
            }

            //3. Structure - Kết cấu
            if (request.StructureImage == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, AppConstant.ErrMessage.Invalid_Structure);
            }

            //4. Electricity Water - Điện & nước
            if (request.ElectricityWaterImage == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Bad_Request, AppConstant.ErrMessage.Invalid_ElectricityWater);
            }

            var projectInfo = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(predicate: p => p.Id == request.ProjectId,
                                                                                             include: p => p.Include(p => p.Customer!));

            var result = await _drawingService.CreateProjectHaveDrawing(request.ProjectId, (Guid)projectInfo.Customer!.AccountId!, request);

            return result;
        }

        public async Task<IPaginate<ProjectResponse>> FilterProjectsMultiParams(
                int page,
                int size,
                DateTime? startTime,
                string? status,
                string? type,
                string? code,
                string? phone)
        {
            Expression<Func<Project, bool>> predicate = x => true;

            if (!string.IsNullOrEmpty(status))
            {
                predicate = predicate.And(x => x.Status != null && x.Status.ToUpper() == status.ToUpper());
            }

            if (!string.IsNullOrEmpty(type))
            {
                predicate = predicate.And(x => x.Type != null && x.Type.ToUpper() == type.ToUpper());
            }

            if (!string.IsNullOrEmpty(code))
            {
                predicate = predicate.And(x => x.ProjectCode != null && x.ProjectCode.Contains(code));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                predicate = predicate.And(x => x.Customer != null && x.Customer.PhoneNumber!.Contains(phone));
            }

            if (startTime.HasValue)
            {
                predicate = predicate.And(x => x.InsDate >= startTime.Value);
            }

            // Thực hiện query
            var listProjects = await _unitOfWork.GetRepository<Project>().GetList(
                predicate: predicate,
                selector: x => new ProjectResponse(x.Id, x.CustomerName!, x.Name, x.Type,
                                                   x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer!),
                orderBy: x => x.OrderByDescending(w => w.InsDate),
                page: page,
                size: size
            );

            return listProjects;
        }

        public async Task<string> GetStatusProjectDetail(Guid projectId)
        {
            var project = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                            predicate: p => p.Id == projectId);
            if (project == null)
            {
                throw new MessageError((int)AppConstant.ErrCode.NotFound, AppConstant.ErrMessage.ProjectNotExit);
            }
            var result = project.Status;
            return result.ToString();
        }
    }
}
