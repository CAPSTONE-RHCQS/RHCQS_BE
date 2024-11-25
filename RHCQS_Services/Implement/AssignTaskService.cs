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

        public async Task<List<AccountResponse>> ListStaffSalesAvailable()
        {
            try
            {
                var listSales = await _unitOfWork.GetRepository<Account>().GetListAsync(
                predicate: x => x.RoleId == Guid.Parse("9959CE96-DE26-40A7-B8A7-28A704062E89"),
                include: x => x.Include(x => x.AssignTasks!)
                               .ThenInclude(assignTask => assignTask.Project!)
                               .Include(x => x.Role));

                var accounts = listSales.Where(account => account.AssignTasks
                                                .Select(assignTask => assignTask.ProjectId)
                                   .Distinct().Count() < 2);

                var accountResponses = accounts.Select(account => new AccountResponse(
                    id: account.Id,
                    username: account.Username,
                    phoneNumber: account.PhoneNumber,
                    dateOfBirth: account.DateOfBirth,
                    passwordHash: account.PasswordHash,
                    email: account.Email,
                    imageUrl: account.ImageUrl,
                    deflag: account.Deflag,
                    rolename: account.Role?.RoleName, 
                    roleId: account.RoleId,
                    insDate: account.InsDate,
                    upsDate: account.UpsDate
                )).ToList();

                return accountResponses; 
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching staff sales: {ex.Message}");
            }
        }


        public async Task<List<DesignStaffWorkResponse>> ListDesignStaffWorkAvailable()
        {
            var listDesign = await _unitOfWork.GetRepository<Account>().GetListAsync(
                predicate: x => x.RoleId == Guid.Parse("7AF0D75E-1157-48B4-899D-3196DEED5FAD"),
                include: x => x.Include(x => x.AssignTasks!)
                               .ThenInclude(assignTask => assignTask.Project!)
                               .Include(x => x.Role)
            );

            var listResult = listDesign
                .Where(account => account.AssignTasks
                    .Select(assignTask => assignTask.ProjectId)
                    .Distinct().Count() < 2)
                .Select(account => new DesignStaffWorkResponse
                {
                    Id = account.Id,
                    ImgUrl = account.ImageUrl,
                    Name = account.Username!,
                    RoleName = account.Role.RoleName!,
                    Phone = account.PhoneNumber
                })
                .ToList();

            return listResult;
        }

    }
}
