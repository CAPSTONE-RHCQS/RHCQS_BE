using Microsoft.Extensions.Logging;
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

        //Confirm design contract's payment of customer
    }
}
