using Azure;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static RHCQS_BusinessObjects.AppConstant;

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

        public async Task<IPaginate<AccountResponse>> GetListAccountAsync(int page, int size)
        {
            try
            {
                IPaginate<AccountResponse> listAccounts = await _unitOfWork.GetRepository<Account>()
                    .GetList(selector: x => new AccountResponse(x.Id, x.Username, x.PhoneNumber, x.DateOfBirth, x.PasswordHash,
                                                                x.Email, x.ImageUrl, x.Deflag, x.RoleId, x.InsDate, x.UpsDate),
                            page: page,
                            size: size);
                return listAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all accounts");
                throw;
            }
        }

        public async Task<int> GetTotalAccountCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
