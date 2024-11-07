﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_Services.Implement
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProjectService> _logger;
        public IAuthService _authService { get; private set; }

        public ProjectService(IUnitOfWork unitOfWork, ILogger<ProjectService> logger, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _authService = authService;
        }

        public async Task<IPaginate<ProjectResponse>> GetProjects(int page, int size)
        {
            IPaginate<ProjectResponse> listProjects =
            await _unitOfWork.GetRepository<Project>().GetList(
                selector: x => new ProjectResponse(x.Id, x.Customer!.Username!, x.Name, x.Type,
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
                selector: x => new ProjectResponse(x.Id, x.Customer!.Username!, x.Name, x.Type,
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
                selector: x => new ProjectResponse(x.Project!.Id, x.Project!.Customer!.Username!, x.Project.Name, x.Project.Type,
                                                    x.Project.Status, x.Project.InsDate, x.Project.UpsDate, x.Project.ProjectCode),
                include: x => x.Include(x => x.Project!)
                                .ThenInclude(x => x.Customer!),
                orderBy: x => x.OrderBy(x => x.InsDate)
                               .ThenBy(x => x.Project!.Status == AppConstant.ProjectStatus.PROCESSING),
                page: page,
                size: size
            );


            List<ProjectResponse> lists = paginatedProjects.Items.ToList();

            return paginatedProjects;
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
                                        }).ToList() ?? new List<InitialInfo>();

            var finalItem = projectItem.FinalQuotations?
                              .Select(d => new FinalInfo
                              {
                                  Id = d.Id,
                                  AccountName = projectItem.AssignTasks.FirstOrDefault()?.Account.Username,
                                  Version = d.Version,
                                  InsDate = d.InsDate,
                                  Status = d.Status
                              }).ToList() ?? new List<FinalInfo>();


            var houseDesignItem = projectItem.HouseDesignDrawings?
                                                                     .Select(h => new HouseDesignDrawingInfo
                                                                     {
                                                                         Id = h.Id,
                                                                         //Version = h.HouseDesignVersions,
                                                                         Step = h.Step,
                                                                         Name = h.Name!,
                                                                         Type = h.Type!,
                                                                         InsDate = h.InsDate,
                                                                         Status = h.Status
                                                                     }).OrderBy(h => h.Step).ToList() ?? new List<HouseDesignDrawingInfo>();

            var contractItem = projectItem.Contracts?.Select(c => new ContractInfo
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status,
                Note = c.Note
            }).ToList() ?? new List<ContractInfo>();


            var projectDetailItem = new ProjectDetail
            {
                Id = projectItem.Id,
                Name = projectItem.Name,
                AccountName = projectItem.Customer!.Username!,
                Address = projectItem.Address,
                Area = projectItem.Area,
                Type = projectItem.Type,
                Status = projectItem.Status,
                InsDate = projectItem.InsDate,
                UpsDate = projectItem.UpsDate,
                ProjectCode = projectItem.ProjectCode,
                StaffName = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.Username)),
                StaffPhone = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.PhoneNumber)),
                StaffAvatar = string.Join(", ", projectItem.AssignTasks.Select(x => x.Account.ImageUrl)),
                InitialInfo = initialItem,
                HouseDesignDrawingInfo = houseDesignItem,
                FinalInfo = finalItem,
                ContractInfo = contractItem,
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
                var projectItem = new Project
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerInfo.Id,
                    Name = "Báo giá sơ bộ " + DateTime.Now,
                    Type = projectRequest.Type,
                    Status = AppConstant.ProjectStatus.PROCESSING,
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    ProjectCode = GenerateRandom.GenerateRandomString(5),
                    Address = projectRequest.Address,
                    Area = projectRequest.Area
                };
                await _unitOfWork.GetRepository<Project>().InsertAsync(projectItem);

                var initialItem = new InitialQuotation
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectItem.Id,
                    PromotionId = projectRequest.InitialQuotation.PromotionId,
                    Area = projectRequest.Area,
                    TimeProcessing = null,
                    TimeRough = null,
                    TimeOthers = null,
                    InsDate = DateTime.Now,
                    Status = AppConstant.QuotationStatus.PENDING,
                    Version = 0.0,
                    IsTemplate = false,
                    Deflag = false,
                    Note = null,
                    ReasonReject = null
                };

                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);

                //PackageQuotation
                if (projectRequest.PackageQuotations.Count  < 1)
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
                        InsDate = DateTime.Now
                    };

                    await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageQuotation);
                }

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
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                        InitialQuotationId = initialItem.Id
                    };
                    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(initialQuotationItem);
                }

                if (projectRequest.PackageQuotations.Count > 0)
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
                                Coefiicient = 0,
                                Price = utl.Price,
                                Description = sectionItem.Description,
                                InsDate = DateTime.Now,
                                UpsDate = DateTime.Now,
                                UtilitiesSectionId = sectionItem.Id
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
                                Coefiicient = utilityItem.Coefficient,
                                Price = utl.Price,
                                Description = utilityItem.Name,
                                InsDate = DateTime.Now,
                                UpsDate = DateTime.Now,
                                UtilitiesSectionId = utilityItem.SectionId
                            };
                        }

                        await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
                    }
                }

                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<string> AssignQuotation(Guid accountId, Guid projectId)
        {
            var projectCount = await _unitOfWork.GetRepository<AssignTask>()
                                                .CountAsync(a => a.AccountId == accountId);

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
                InsDate = DateTime.Now
            };

            //var initialInfo = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.Version == 0.0);

            await _unitOfWork.GetRepository<AssignTask>().InsertAsync(assignItem);

            //Update Initial quotation status PENDING -> PROCESSING
            var initialInfo = await _unitOfWork.GetRepository<InitialQuotation>().FirstOrDefaultAsync(x => x.ProjectId == projectId);
            initialInfo.Status = AppConstant.QuotationStatus.PROCESSING;
            _unitOfWork.GetRepository<InitialQuotation>().UpdateAsync(initialInfo);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful ? "Phân công Sales thành công!" : throw new Exception("Phân công thất bại!");
        }

        public async Task<bool> CancelProject(Guid projectId)
        {
            var infoProject = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
                            predicate: x => x.Id == projectId,
                            include: x => x.Include(x => x.InitialQuotations)
                                           .Include(x => x.FinalQuotations)
                                           .Include(x => x.HouseDesignDrawings)
                );
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

            var initialQuotation = await _unitOfWork.GetRepository<InitialQuotation>()
                .FirstOrDefaultAsync(
                    predicate: q => q.ProjectId == projectId,
                    orderBy: q => q.OrderByDescending(q => q.Version)
                );

            var initialResponse = initialQuotation != null
                ? new InitialAppResponse { Status = initialQuotation.Status! }
                : null;

            var finalQuotation = await _unitOfWork.GetRepository<FinalQuotation>()
               .FirstOrDefaultAsync(
                   predicate: q => q.ProjectId == projectId,
                   orderBy: q => q.OrderByDescending(q => q.Version)
               );

            var finalResponse = finalQuotation != null
                ? new FinalAppResponse { Status = finalQuotation.Status! }
                : null;

            var contracts = await _unitOfWork.GetRepository<Contract>()
               .GetListAsync(
                   predicate: c => c.ProjectId == projectId
            );

            var contractDesignResponse = contracts
              .Where(c => c.Type == AppConstant.ContractType.Design.ToString())
              .Select(c => new ContractDesignAppResponse { Status = c.Status })
                    .FirstOrDefault();

            var processingResponse = contracts
                .Where(c => c.Type == AppConstant.ContractType.Construction.ToString())
                .Select(c => new ContractProcessingAppResponse { Status = c.Status })
                .FirstOrDefault();

            return new ProjectAppResponse(
                initialResponse,
                contractDesignResponse,
                finalResponse,
                processingResponse
            );
        }

        public async Task<bool> CreateProjectTemplateHouse(TemplateHouseProjectRequest request)
        {
            try
            {
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


                var projectItem = new Project
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerInfo.Id,
                    Name = "Báo giá sơ bộ " + DateTime.Now,
                    Type = AppConstant.Type.TEMPLATE,
                    Status = AppConstant.ProjectStatus.PROCESSING,
                    InsDate = DateTime.Now,
                    UpsDate = DateTime.Now,
                    ProjectCode = GenerateRandom.GenerateRandomString(5),
                    Address = request.Address,
                    Area = templateHouseInfo.BuildingArea
                };
                await _unitOfWork.GetRepository<Project>().InsertAsync(projectItem);


                //System find package rough
                var packageRough = await _unitOfWork.GetRepository<Package>().FirstOrDefaultAsync(
                                        predicate: p => p.PackageType.Name == AppConstant.Type.ROUGH
                                        && p.PackageHouses.Any(p => p.DesignTemplateId == p.DesignTemplateId));

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

                var initialItem = new InitialQuotation
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectItem.Id,
                    PromotionId = null,
                    Area = templateHouseInfo.BuildingArea,
                    TimeProcessing = null,
                    TimeRough = null,
                    TimeOthers = null,
                    InsDate = DateTime.Now,
                    Status = AppConstant.QuotationStatus.PENDING,
                    Version = 0.0,
                    IsTemplate = true,
                    Deflag = false,
                    Note = null,
                    TotalRough = totalPriceRough,
                    ReasonReject = null
                };

                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);


                var packageRoughQuotation = new PackageQuotation
                {
                    Id = Guid.NewGuid(),
                    PackageId = packageRough.Id,
                    InitialQuotationId = initialItem.Id,
                    Type = AppConstant.Type.ROUGH,
                    InsDate = DateTime.Now
                };

                await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageRoughQuotation);

                //Customer choose package finished
                var packageFinishedQuotation = new PackageQuotation
                {
                    Id = Guid.NewGuid(),
                    PackageId = request.PackageFinished,
                    InitialQuotationId = initialItem.Id,
                    Type = AppConstant.Type.FINISHED,
                    InsDate = DateTime.Now
                };

                await _unitOfWork.GetRepository<PackageQuotation>().InsertAsync(packageFinishedQuotation);

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
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                        InitialQuotationId = initialItem.Id
                    };
                    await _unitOfWork.GetRepository<InitialQuotationItem>().InsertAsync(initialQuotationItem);
                }

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
                                Coefiicient = 0,
                                Price = utl.Price,
                                Description = sectionItem.Description,
                                InsDate = DateTime.Now,
                                UpsDate = DateTime.Now,
                                UtilitiesSectionId = sectionItem.Id
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
                                Coefiicient = utilityItem.Coefficient,
                                Price = utl.Price,
                                Description = null,
                                InsDate = DateTime.Now,
                                UpsDate = DateTime.Now,
                                UtilitiesSectionId = utilityItem.SectionId
                            };
                        }

                        await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
                    }
                }

                //Promotion ....

                var saveResutl = await _unitOfWork.CommitAsync() > 0 ? AppConstant.Message.SUCCESSFUL_SAVE : AppConstant.ErrMessage.Fail_Save;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            try
            {
                // Kiểm tra và lấy dự án
                var project = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(p => p.Id == projectId);
                if (project == null)
                {
                    throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);
                }

                // Lấy InitialQuotations liên quan đến dự án
                var initialQuotations = await _unitOfWork.GetRepository<InitialQuotation>()
                    .GetListAsync(predicate: iq => iq.ProjectId == projectId);

                if (initialQuotations != null && initialQuotations.Any())
                {
                    // Lấy danh sách ID của các InitialQuotation để sử dụng trong điều kiện Contains
                    var initialQuotationIds = initialQuotations.Select(iq => iq.Id).ToList();

                    // Xóa QuotationUtilities
                    var quotationUtilities = await _unitOfWork.GetRepository<QuotationUtility>()
                       .GetListAsync(predicate: qu => qu.InitialQuotationId.HasValue && initialQuotationIds.Contains(qu.InitialQuotationId.Value));

                    foreach (var utility in quotationUtilities)
                    {
                        _unitOfWork.GetRepository<QuotationUtility>().DeleteAsync(utility);
                    }

                    // Xóa InitialQuotationItems
                    var initialQuotationItems = await _unitOfWork.GetRepository<InitialQuotationItem>()
                        .GetListAsync(predicate: iqi => initialQuotationIds.Contains(iqi.InitialQuotationId));

                    foreach (var item in initialQuotationItems)
                    {
                        _unitOfWork.GetRepository<InitialQuotationItem>().DeleteAsync(item);
                    }

                    // Xóa PackageQuotations
                    var packageQuotations = await _unitOfWork.GetRepository<PackageQuotation>()
                        .GetListAsync(predicate: pq => initialQuotationIds.Contains(pq.InitialQuotationId));

                    foreach (var package in packageQuotations)
                    {
                        _unitOfWork.GetRepository<PackageQuotation>().DeleteAsync(package);
                    }

                    // Xóa InitialQuotations
                    foreach (var initialQuotation in initialQuotations)
                    {
                        _unitOfWork.GetRepository<InitialQuotation>().DeleteAsync(initialQuotation);
                    }
                }

                //Tìm Assign Task
                var taskInfo = await _unitOfWork.GetRepository<AssignTask>().FirstOrDefaultAsync(x => x.ProjectId == projectId);
                _unitOfWork.GetRepository<AssignTask>().DeleteAsync(taskInfo);

                //Tìm House design drawing
                var houseDrawingInfo = await _unitOfWork.GetRepository<HouseDesignDrawing>().GetListAsync(predicate: x => x.ProjectId == projectId,
                                                    include: x => x.Include(x => x.HouseDesignVersions)
                                                                    .ThenInclude(x => x.Media));
                await _unitOfWork.GetRepository<HouseDesignDrawing>().DeleteRangeAsync(houseDrawingInfo);

                //Contract 
                var contractInfo = await _unitOfWork.GetRepository<Contract>().FirstOrDefaultAsync(x => x.ProjectId == projectId &&
                                                        x.Type == "Design",
                                                        include: x => x.Include(x => x.BatchPayments)
                                                                        .ThenInclude(x => x.Payment!));
                 _unitOfWork.GetRepository<Contract>().DeleteAsync(contractInfo);

                // Xóa dự án
                _unitOfWork.GetRepository<Project>().DeleteAsync(project);

                // Lưu thay đổi
                var result = await _unitOfWork.CommitAsync() > 0;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
