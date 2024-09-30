using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public ProjectService(IUnitOfWork unitOfWork, ILogger<ProjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<ProjectResponse>> GetProjects(int page, int size)
        {
            IPaginate<ProjectResponse> listProjects =
            await _unitOfWork.GetRepository<Project>().GetList(
                selector: x => new ProjectResponse(x.Id, x.Customer.Username, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer),
                orderBy: x => x.OrderBy(w => w.InsDate),
                page: page,
                size: size
                );
            return listProjects;
        }

        public async Task<ProjectDetail> GetDetailProjectById(Guid id)
        {
            var projectItem = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(w => w.Id == id,
                                include: w => w.Include(p => p.Customer)
                                                .Include(p => p.InitialQuotations)
                                                .Include(p => p.FinalQuotations)
                                                .Include(p => p.HouseDesignDrawings)
                                                .ThenInclude(h => h.HouseDesignVersions)
                                                .Include(p => p.HouseDesignDrawings)
                                                .ThenInclude(h => h.AssignTask));
            if (projectItem == null) throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.ProjectNotExit);

            if (projectItem == null) return null;

            var initialItem = projectItem.InitialQuotations?
                                        .Select(i => new InitialInfo
                                        {
                                            Id = i.Id,
                                            AccountName = i.Account?.Username,
                                            Version = i.Version,
                                            InsDate = i.InsDate,
                                            Status = i.Status
                                        }).ToList() ?? new List<InitialInfo>();

            var finalItem = projectItem.FinalQuotations?
                                        .Select(d => new FinalInfo
                                        {
                                            Id = d.Id,
                                            AccountName = d.Account?.Username,
                                            Version = d.Version,
                                            InsDate = d.InsDate,
                                            Status = d.Status
                                        }).ToList() ?? new List<FinalInfo>();

            var houseDesignItem = projectItem.HouseDesignDrawings?
                                                                     .Select(h => new HouseDesignDrawingInfo
                                                                     {
                                                                         Id = h.Id,
                                                                         //Version = h.HouseDesignVersions.,
                                                                         InsDate = h.InsDate,
                                                                         Status = h.Status
                                                                     }).ToList() ?? new List<HouseDesignDrawingInfo>();


            var projectDetailItem = new ProjectDetail
            {
                Id = projectItem.Id,
                Name = projectItem.Name,
                AccountName = projectItem.Customer.Username,
                Address = projectItem.Address,
                Area = projectItem.Area,
                Type = projectItem.Type,
                Status = projectItem.Status,
                InsDate = projectItem.InsDate,
                UpsDate = projectItem.UpsDate,
                ProjectCode = projectItem.ProjectCode,
                InitialInfo = initialItem,
                HouseDesignDrawingInfo = houseDesignItem,
                FinalInfo = finalItem
            };

            return projectDetailItem;
        }

        public async Task<List<ProjectResponse>> SearchProjectByPhone(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                throw new AppConstant.MessageError((int)AppConstant.ErrCode.Not_Found, AppConstant.ErrMessage.PhoneIsEmpty);
            }

            var projectPaginate = await _unitOfWork.GetRepository<Project>().GetList(
                selector: x => new ProjectResponse(x.Id, x.Customer.Username, x.Name, x.Type,
                                                    x.Status, x.InsDate, x.UpsDate, x.ProjectCode),
                include: x => x.Include(w => w.Customer),
                predicate: x => x.Customer.PhoneNumber == phoneNumber,
                orderBy: x => x.OrderBy(w => w.InsDate));


            return projectPaginate.Items.ToList();
        }
    }
}
