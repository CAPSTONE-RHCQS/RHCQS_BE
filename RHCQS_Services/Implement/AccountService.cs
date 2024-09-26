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

namespace RHCQS_Services.Implement
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IUnitOfWork unitOfWork, ILogger<AccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetActiveAccountCountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            try
            {
                var accountRepository = _unitOfWork.GetRepository<Account>();
                var accounts = await accountRepository.GetListAsync();
                return accounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all accounts");
                throw;
            }
        }

        public async Task<IPaginate<AccountResponse>> GetListAccountAsync(int page, int size)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalAccountCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
