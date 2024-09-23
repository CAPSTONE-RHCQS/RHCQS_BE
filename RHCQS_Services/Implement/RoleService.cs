using Microsoft.Extensions.Logging;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.Repo.Interface;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.RoleRepository.GetListAsync();
        }

        public async Task<int> GetTotalRoleCountAsync()
        {
            var totalRole = await _unitOfWork.AccountRepository.CountAsync();
            return totalRole;
        }
    }
}
