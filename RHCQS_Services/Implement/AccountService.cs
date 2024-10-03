using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
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
            if (id == Guid.Empty)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.AccountIdError
                );
            }
            var accountRepository = _unitOfWork.GetRepository<Account>();
            var account = await accountRepository.FirstOrDefaultAsync(a => a.Id == id, include: q => q.Include(x => x.Role));
            if (account == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Account
                );
            }
            return account;
        }

        public async Task<int> GetActiveAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            return await accountRepository.CountAsync(a => a.Deflag == true);
        }

        public async Task<IPaginate<AccountResponse>> GetListAccountAsync(int page, int size)
        {

            if (page < 1 || size < 1)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.PageAndSizeError
                );
            }
            IPaginate<AccountResponse> listAccounts = await _unitOfWork.GetRepository<Account>()
                .GetList(selector: x => new AccountResponse(x.Id, x.Username, x.PhoneNumber, x.DateOfBirth, x.PasswordHash,
                                                            x.Email, x.ImageUrl, x.Deflag, x.Role.RoleName, x.RoleId, x.InsDate, x.UpsDate),
                        page: page,
                        size: size);
            if (listAccounts == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.FailedToGetList
                );
            }
            return listAccounts;

        }
        public async Task<IPaginate<AccountResponse>> GetListAccountByRoleIdAsync(Guid id,int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }
            if (page < 1 || size < 1)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.PageAndSizeError
                );
            }
            IPaginate<AccountResponse> listAccounts = await _unitOfWork.GetRepository<Account>()
                .GetList(selector: x => new AccountResponse(x.Id, x.Username, x.PhoneNumber, x.DateOfBirth, x.PasswordHash,
                                                            x.Email, x.ImageUrl, x.Deflag, x.Role.RoleName, x.RoleId, x.InsDate, x.UpsDate),
                        predicate: x => x.RoleId == id,
                        page: page,
                        size: size);
            if (listAccounts == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.FailedToGetList
                );
            }
            return listAccounts;

        }
        public async Task<int> GetTotalAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            return await accountRepository.CountAsync();
        }
        public async Task<Account> SearchAccountsByNameAsync(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                return await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(include: s => s.Include(p => p.Role));
            }
            else
            {
                var account = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(p => p.Username.Contains(name), include: s => s.Include(p => p.Role));
                if (account == null)
                {
                    throw new AppConstant.MessageError(
                        (int)AppConstant.ErrCode.Not_Found,
                        AppConstant.ErrMessage.Not_Found_Account
                    );
                }
                return account;
            }
        }

        public async Task<Account> UpdateAccountAsync(Guid id, Account account)
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            var _account = await accountRepository.FirstOrDefaultAsync(a => a.Id == id, include: q => q.Include(x => x.Role));

            if (_account == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Account
                );
            }
            _account.Username = account.Username != default ? account.Username : _account.Username;
            _account.PhoneNumber = account.PhoneNumber != default ? account.PhoneNumber : _account.PhoneNumber;
            _account.DateOfBirth = account.DateOfBirth != default ? account.DateOfBirth : _account.DateOfBirth;
            _account.PasswordHash = PasswordHash.HashPassword(account.PasswordHash) != default ? PasswordHash.HashPassword(account.PasswordHash) : _account.PasswordHash;
            _account.Deflag = account.Deflag != default ? account.Deflag : _account.Deflag;
            _account.RoleId = account.RoleId != Guid.Empty ? account.RoleId : _account.RoleId;
            _account.ImageUrl = account.ImageUrl == default ? null : account.ImageUrl;
            _account.UpsDate = DateTime.UtcNow;

            accountRepository.UpdateAsync(_account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.UpdateAccount
                );
            }

            return _account;
        }
        public async Task<Account> UpdateDeflagAccountAsync(Guid id)
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            var account = await accountRepository.FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Account
                );
            }

            account.Deflag = false;
            account.UpsDate = DateTime.UtcNow;

            accountRepository.UpdateAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.BanAccount
                );
            }

            return account;
        }
    }
}
