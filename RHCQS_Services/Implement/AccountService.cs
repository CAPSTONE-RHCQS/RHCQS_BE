using Azure;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var accountRepository = _unitOfWork.GetRepository<Account>();
                var account = await accountRepository.FirstOrDefaultAsync(a => a.Id == id, include: q => q.Include(x => x.Role));
                if (account == null)
                {
                    throw new KeyNotFoundException("Account not found");
                }
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting the account by ID");
                throw;
            }
        }

        public async Task<int> GetActiveAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();
            return await accountRepository.CountAsync(a => a.Deflag == true);
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
            var accountRepository = _unitOfWork.GetRepository<Account>();
            return await accountRepository.CountAsync();
        }
        public async Task<Account> SearchAccountsByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(include: s => s.Include(p => p.Role));
                }
                else
                {
                    return await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(p => p.Username.Contains(name), include: s => s.Include(p => p.Role));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the account by name.");
                throw;
            }
        }

        public async Task<Account> UpdateAccountAsync(Guid id, Account account, Stream imageStream, string imageName)
        {
            try
            {
                var accountRepository = _unitOfWork.GetRepository<Account>();
                var _account = await accountRepository.FirstOrDefaultAsync(a => a.Id == id, include: q => q.Include(x => x.Role));

                if (_account == null)
                {
                    return null;
                }

                _account.Username = account.Username != default ? account.Username : _account.Username;
                _account.PhoneNumber = account.PhoneNumber != default ? account.PhoneNumber : _account.PhoneNumber;
                _account.DateOfBirth = account.DateOfBirth != default ? account.DateOfBirth : _account.DateOfBirth;
                _account.PasswordHash = account.PasswordHash != default ? account.PasswordHash : _account.PasswordHash;
                _account.Deflag = account.Deflag != default ? account.Deflag : _account.Deflag;
                _account.RoleId = account.RoleId != Guid.Empty ? account.RoleId : _account.RoleId;

/*                if (imageStream != null)
                {
                    var imageUrl = await UploadImageToFirebase(imageStream, imageName);
                    _account.ImgUrl = imageUrl;
                }
                else if (imageStream == null)
                {
                    account.ImgUrl = _account.ImgUrl;
                }*/
                _account.UpsDate = DateTime.UtcNow;

                accountRepository.UpdateAsync(_account);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful) return null;

                return _account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating account");
                throw;
            }
        }
        public async Task<Account> UpdateDeflagAccountAsync(Guid id)
        {
            try
            {
                var accountRepository = _unitOfWork.GetRepository<Account>();
                var account = await accountRepository.FirstOrDefaultAsync(a => a.Id == id);

                if (account == null)
                {
                    return null;
                }

                account.Deflag = false;
                account.UpsDate = DateTime.UtcNow;

                accountRepository.UpdateAsync(account);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccessful)
                {
                    return null;
                }

                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating deflag of account");
                throw;
            }
        }
    }
}
