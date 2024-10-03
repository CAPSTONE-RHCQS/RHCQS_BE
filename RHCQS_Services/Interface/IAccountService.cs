using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IAccountService
    {
        Task<Account> GetAccountByIdAsync(Guid id);
        Task<int> GetTotalAccountCountAsync();
        Task<int> GetActiveAccountCountAsync();
        public Task<IPaginate<AccountResponse>> GetListAccountAsync(int page, int size);
        public Task<IPaginate<AccountResponse>> GetListAccountByRoleIdAsync(Guid id,int page, int size);
        Task<Account> SearchAccountsByNameAsync(string name);
        Task<Account> UpdateAccountAsync(Guid id, Account account);
        Task<Account> UpdateDeflagAccountAsync(Guid id);
    }
}
