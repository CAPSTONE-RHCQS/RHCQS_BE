using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
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
                var projectItem = new Project
                {
                    Id = Guid.NewGuid(),
                    CustomerId = projectRequest.CustomerId,
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
                    Version = 1.0,
                    IsTemplate = false,
                    Deflag = false,
                    Note = null,
                    ReasonReject = null
                };

                await _unitOfWork.GetRepository<InitialQuotation>().InsertAsync(initialItem);
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

                foreach (var utl in projectRequest.QuotationUtilitiesRequest!)
                {
                    var utlItem = new QuotationUtility
                    {
                        Id = Guid.NewGuid(),
                        UtilitiesItemId = utl.UltilitiesItemId,
                        FinalQuotationId = null,
                        InitialQuotationId = initialItem.Id,
                        Name = "",
                        Coefiicient = utl.Coefiicient,
                        Price = utl.Price,
                        Description = utl.Description,
                        InsDate = DateTime.Now,
                        UpsDate = DateTime.Now,
                    };
                    await _unitOfWork.GetRepository<QuotationUtility>().InsertAsync(utlItem);
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

            await _unitOfWork.GetRepository<AssignTask>().InsertAsync(assignItem);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful ? "Phân công Sales thành công!" : throw new Exception("Phân công thất bại!");
        }

        //Cancel project    
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
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
