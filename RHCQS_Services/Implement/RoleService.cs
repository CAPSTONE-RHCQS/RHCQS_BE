using Azure;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.Repo.Interface;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IUnitOfWork unitOfWork, ILogger<RoleService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IPaginate<RoleResponse>> GetAllRolesAsync(int page, int size)
        {
            IPaginate<RoleResponse> listRoles =
            await _unitOfWork.GetRepository<Role>().GetList(
                selector: x => new RoleResponse(x.Id, x.RoleName),
                page: page,
                size: size
                );
            return listRoles;
        }

        public async Task<int> GetTotalRoleCountAsync()
        {
            var totalRole = await _unitOfWork.GetRepository<Role>().CountAsync();
            return totalRole;
        }
    }
}
