using CloudinaryDotNet.Actions;
using Google.Api.Gax.Rest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class AssignTaskService : IAssignTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IAssignTaskService> _logger;

        public AssignTaskService(IUnitOfWork unitOfWork, ILogger<IAssignTaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<AccountResponse>> ListStaffSalesAvailable(int page, int size)
        {
            try
            {
                IPaginate<AccountResponse> listSales = await _unitOfWork.GetRepository<Account>().GetList(
              predicate: x => x.RoleId == Guid.Parse("9959CE96-DE26-40A7-B8A7-28A704062E89") && x.Deflag != false &&
                x.AssignTasks.Count(at => at.Project.Status == AppConstant.ProjectStatus.PROCESSING) <= 1 &&
                x.AssignTasks.Count(at => at.Project.Status == AppConstant.ProjectStatus.PROCESSING ||
                                          at.Project.Status == AppConstant.ProjectStatus.FINALIZED ||
                                          at.Project.Status == AppConstant.ProjectStatus.ENDED) < 2,
                selector: x => new AccountResponse(x.Id, x.Username, x.PhoneNumber, x.DateOfBirth,
                                                   x.PasswordHash, x.Email, x.ImageUrl, x.Deflag, x.Role.RoleName,
                                                   x.RoleId, x.InsDate, x.UpsDate),
                include: x => x.Include(x => x.AssignTasks!)
                               .ThenInclude(assignTask => assignTask.Project!)
                               .Include(x => x.Role),
                page: page,
                size: size);

                return listSales;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching staff sales: {ex.Message}");
            }
        }

        public async Task<IPaginate<DesignStaffWorkResponse>> ListDesignStaffWorkAvailable(int page, int size)
        {
            IPaginate<DesignStaffWorkResponse> listDesign = await _unitOfWork.GetRepository<Account>().GetList(
                 predicate: x => x.RoleId == Guid.Parse("7AF0D75E-1157-48B4-899D-3196DEED5FAD") && x.Deflag != false &&
                        !x.HouseDesignDrawings.Any(hdd => hdd.Status == AppConstant.HouseDesignStatus.PENDING ||
                                                          hdd.Status == AppConstant.HouseDesignStatus.PROCESSING ||
                                                          hdd.Status == AppConstant.HouseDesignStatus.UPDATING),
                selector: x => new DesignStaffWorkResponse(x.Id, x.ImageUrl, x.Username, x.Role.RoleName, x.PhoneNumber),
                include: x => x.Include(x => x.HouseDesignDrawings!)
                               .ThenInclude(r => r.Account!)
                               .ThenInclude(r => r.Role),
                page: page,
                size: size
            );

            return listDesign;
        }



    }
}
