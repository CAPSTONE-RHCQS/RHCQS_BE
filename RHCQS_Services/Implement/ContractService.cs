using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Implement
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IContractService> _logger;

        public ContractService(IUnitOfWork unitOfWork, ILogger<IContractService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        //Create design contract
        //public async Task<bool> CreateContractDeisgn(Guid projectId, string type)
        //{
        //    var infoProjec = await _unitOfWork.GetRepository<Project>().FirstOrDefaultAsync(
        //                        predicate: x => x.Id == projectId,
        //                        include: x => x.Include(x => x.InitialQuotations)
        //                                        .ThenInclude(x => x.PackageQuotations)
        //                                        .Include(x => x.Customer!)
        //        );

        //    return true;
        //}

        //Confirm design contract's payment of customer
    }
}
