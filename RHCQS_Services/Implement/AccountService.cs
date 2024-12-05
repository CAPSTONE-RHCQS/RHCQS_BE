using Azure;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObject.Payload.Response.CurrentUserModel;
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
using Account = RHCQS_DataAccessObjects.Models.Account;
using Role = RHCQS_DataAccessObjects.Models.Role;

namespace RHCQS_Services.Implement
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountService> _logger;
        private readonly IUploadImgService _uploadImgService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IUnitOfWork unitOfWork, 
            ILogger<AccountService> logger, 
            IUploadImgService uploadImgService, 
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _uploadImgService = uploadImgService;
            _httpContextAccessor = httpContextAccessor;
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
            var account = await accountRepository.FirstOrDefaultAsync(a => a.Id == id && a.Deflag == true,
                include: q => q.Include(x => x.Role));
            if (account == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Account
                );
            }
            return account;
        }
        public async Task<AccountCustomerResponse> GetAccountOrCustomerByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.AccountIdError
                );
            }

            var accountRepository = _unitOfWork.GetRepository<Account>();
            var account = await accountRepository.FirstOrDefaultAsync(
                a => a.Id == id && a.Deflag == true,
                include: q => q.Include(x => x.Role)
            );

            if (account != null)
            {
                return new AccountCustomerResponse
                {
                    Id = account.Id,
                    Email = account.Email,
                    Username = account.Username,
                    PhoneNumber = account.PhoneNumber,
                    ImageUrl = account.ImageUrl,
                    DateOfBirth = account.DateOfBirth?.ToDateTime(TimeOnly.MinValue),
                    Deflag = account.Deflag,
                    RoleName = account.Role?.RoleName
                };
            }

            var customerRepository = _unitOfWork.GetRepository<Customer>();
            var customer = await customerRepository.FirstOrDefaultAsync(
                c => c.Id == id && c.Deflag == true
            );

            if (customer != null)
            {
                return new AccountCustomerResponse
                {
                    Id = customer.Id,
                    Email = customer.Email,
                    Username = customer.Username,
                    PhoneNumber = customer.PhoneNumber,
                    ImageUrl = customer.ImgUrl,
                    DateOfBirth = customer.DateOfBirth,
                    Deflag = customer.Deflag,
                    RoleName = "Customer"
                };
            }

            throw new AppConstant.MessageError(
                (int)AppConstant.ErrCode.Not_Found,
                AppConstant.ErrMessage.Not_Found_Account
            );
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
                        predicate: x => x.Deflag == true,
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
            var role = _unitOfWork.GetRepository<Role>();
            var _role = await role.FirstOrDefaultAsync(a => a.Id == id);

            if (_role == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Role
                );
            }
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
                        predicate: x => x.RoleId == id && x.Deflag == true,
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
                var account = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(p => p.Username.Contains(name) && p.Deflag == true,
                    include: s => s.Include(p => p.Role));
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

        public async Task<Account> UpdateAccountAsync(Guid id, AccountRequestForUpdate account)
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
            _account.Deflag = account.Deflag != default ? account.Deflag : _account.Deflag;
            _account.UpsDate = LocalDateTime.VNDateTime();

            accountRepository.UpdateAsync(_account);
            if (_account.Role.RoleName == UserRoleForRegister.Customer.ToString())
            {
                var customerRepository = _unitOfWork.GetRepository<Customer>();
                var _customer = await customerRepository.FirstOrDefaultAsync(c => c.Email == _account.Email);

                if (_customer != null)
                {
                    _customer.Username = account.Username != default ? account.Username : _customer.Username;
                    _customer.PhoneNumber = account.PhoneNumber != default ? account.PhoneNumber : _customer.PhoneNumber;
                    _customer.DateOfBirth = account.DateOfBirth != default ? account.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue) : _customer.DateOfBirth;
                    _customer.Deflag = account.Deflag != default ? account.Deflag : _customer.Deflag;
                    _customer.UpsDate = LocalDateTime.VNDateTime();

                    customerRepository.UpdateAsync(_customer);
                }
            }
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
        public async Task<Account> UpdateProfileAsync(Guid id, AccountRequestForUpdateProfile account)
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
            _account.UpsDate = LocalDateTime.VNDateTime();

            accountRepository.UpdateAsync(_account);
            if (_account.Role.RoleName == UserRoleForRegister.Customer.ToString())
            {
                var customerRepository = _unitOfWork.GetRepository<Customer>();
                var _customer = await customerRepository.FirstOrDefaultAsync(c => c.Email == _account.Email);

                if (_customer != null)
                {
                    _customer.Username = account.Username != default ? account.Username : _customer.Username;
                    _customer.PhoneNumber = account.PhoneNumber != default ? account.PhoneNumber : _customer.PhoneNumber;
                    _customer.DateOfBirth = account.DateOfBirth != default ? account.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue) : _customer.DateOfBirth;
                    _customer.UpsDate = LocalDateTime.VNDateTime();

                    customerRepository.UpdateAsync(_customer);
                }
            }
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
            account.UpsDate = LocalDateTime.VNDateTime();

            accountRepository.UpdateAsync(account);
            if (account.Role.RoleName == UserRoleForRegister.Customer.ToString())
            {
                var customerRepository = _unitOfWork.GetRepository<Customer>();
                var customer = await customerRepository.FirstOrDefaultAsync(c => c.Email == account.Email);

                if (customer != null)
                {
                    customer.Deflag = false;
                    customer.UpsDate = LocalDateTime.VNDateTime();

                    customerRepository.UpdateAsync(customer);
                }
            }
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
        public async Task<bool> UpdatePasswordAsync(Guid id, string currentPassword, string newPassword)
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

            if (!PasswordHash.VerifyPassword(currentPassword, account.PasswordHash))
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Unauthorized,
                    AppConstant.ErrMessage.IncorrectPassword
                );
            }

            account.PasswordHash = PasswordHash.HashPassword(newPassword);
            account.UpsDate = LocalDateTime.VNDateTime();

            accountRepository.UpdateAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.UpdatePasswordFailed
                );
            }

            return true;
        }
        public async Task<bool> CreateImageAccount(Guid accountId, ImageForAccount files)
        {
            var uploadResults = new List<string>();
            string nameImage = null;
            if (files.AccountImage != null)
            {
                nameImage = AppConstant.Profile.IMAGE;
                var accountImg = await _uploadImgService.UploadFileForImageAccount(accountId, files.AccountImage, "profile", nameImage);
                var accountinfo = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(x => x.Id == accountId);
                accountinfo.ImageUrl = accountImg;
                var role = await _unitOfWork.GetRepository<Role>().FirstOrDefaultAsync(x => x.Id == accountinfo.RoleId);
                _unitOfWork.GetRepository<Account>().UpdateAsync(accountinfo);
                if (role.RoleName == UserRoleForRegister.Customer.ToString())
                {
                    var customerRepository = _unitOfWork.GetRepository<Customer>();
                    var _customer = await customerRepository.FirstOrDefaultAsync(c => c.Email == accountinfo.Email);

                    if (_customer != null)
                    {
                        _customer.ImgUrl = accountinfo.ImageUrl;
                        _customer.UpsDate = LocalDateTime.VNDateTime();
                        customerRepository.UpdateAsync(_customer);
                    }
                }
                await _unitOfWork.CommitAsync();
                uploadResults.Add(accountImg);
            }

            return uploadResults.Count > 0;
        }

        public async Task<CurrentUserModel> GetCurrentLoginUser()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                throw new Exception("Cannot find the logged-in user.");
            }

            var currentLoginUser = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                                    predicate: x => x.Id == userId
                                );

            if (currentLoginUser == null)
            {
                throw new Exception("User not found.");
            }

            var currentUserModel = new CurrentUserModel
            {
                Userid = currentLoginUser.Id,
                Username = currentLoginUser.Username,
                UserProfileImage = currentLoginUser.ImageUrl,
                Email = currentLoginUser.Email,
                Phonenumber = currentLoginUser.PhoneNumber,
                IsBan = (bool)currentLoginUser.Deflag! ? false : true
            };

            return currentUserModel;
        }
        public async Task<string> GetEmailByProjectIdAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var projectRepository = _unitOfWork.GetRepository<Project>();
            var project = await projectRepository.FirstOrDefaultAsync(
                p => p.Id == projectId,
                include: q => q.Include(p => p.Customer)
                              .ThenInclude(c => c.Account)
            );

            if (project?.Customer == null || project.Customer.Deflag != true)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.ProjectNotExit
                );
            }

            var account = project.Customer.Account;
            if (account == null || account.Deflag != true)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.Not_Found_Account
                );
            }

            return SanitizeEmail(account.Email);
        }
        public async Task<string> GetEmailByQuotationIdAsync(Guid quotationId)
        {
            if (quotationId == Guid.Empty)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var finalQuotationRepo = _unitOfWork.GetRepository<FinalQuotation>();
            var finalQuotation = await finalQuotationRepo.FirstOrDefaultAsync(
                fq => fq.Id == quotationId,
                include: q => q.Include(fq => fq.Project)
                               .ThenInclude(p => p.Customer)
                               .ThenInclude(c => c.Account)
            );

            if (finalQuotation?.Project?.Customer != null && finalQuotation.Project.Customer.Deflag == true)
            {
                var account = finalQuotation.Project.Customer.Account;
                if (account != null && account.Deflag == true)
                {
                    return SanitizeEmail(account.Email);
                }
            }

            var initialQuotationRepo = _unitOfWork.GetRepository<InitialQuotation>();
            var initialQuotation = await initialQuotationRepo.FirstOrDefaultAsync(
                iq => iq.Id == quotationId,
                include: q => q.Include(iq => iq.Project)
                               .ThenInclude(p => p.Customer)
                               .ThenInclude(c => c.Account)
            );

            if (initialQuotation?.Project?.Customer != null && initialQuotation.Project.Customer.Deflag == true)
            {
                var account = initialQuotation.Project.Customer.Account;
                if (account != null && account.Deflag == true)
                {
                    return SanitizeEmail(account.Email);
                }
            }

            throw new AppConstant.MessageError(
                (int)AppConstant.ErrCode.Not_Found,
                AppConstant.ErrMessage.Not_Found_Account
            );
        }
        public async Task<Account> RegisterForStaffAsync(RegisterRequest registerRequest, UserRoleForManagerRegister selectedrole)
        {
            if (registerRequest == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Bad_Request,
                    AppConstant.ErrMessage.NullValue
                );
            }

            var existingAccount = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                x => x.Email.Equals(registerRequest.Email));

            if (existingAccount != null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.EmailExists
                );
            }

            var existingPhoneNumber = await _unitOfWork.GetRepository<Account>().FirstOrDefaultAsync(
                x => x.PhoneNumber.Equals(registerRequest.PhoneNumber)
            );

            if (existingPhoneNumber != null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.PhoneNumberExists
                );
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.PasswordMismatch
                );
            }

            var roleName = selectedrole.ToString();
            var role = await _unitOfWork.GetRepository<Role>().FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Not_Found,
                    AppConstant.ErrMessage.RoleNotFound
                );
            }
            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                PasswordHash = PasswordHash.HashPassword(registerRequest.Password),
                Username = registerRequest.Email,
                InsDate = LocalDateTime.VNDateTime(),
                UpsDate = LocalDateTime.VNDateTime(),
                RoleId = role.Id,
                Deflag = true
            };

            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new AppConstant.MessageError(
                    (int)AppConstant.ErrCode.Conflict,
                    AppConstant.ErrMessage.CreateAccountError
                );
            }

            return newAccount;
        }
        private string SanitizeEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return string.Empty;

            return email.Replace("@", "_at_").Replace(".", "_dot_");
        }

        public async Task<int> GetSStaffAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(x => x.Role.RoleName == AppConstant.Role.SalesStaff && x.Deflag == true);

            return count;
        }
        public async Task<int> GetDStaffAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(x => x.Role.RoleName == AppConstant.Role.DesignStaff && x.Deflag == true);

            return count;
        }
        public async Task<int> GetCustomerAccountCountAsync()
        {
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(x => x.Role.RoleName == AppConstant.Role.Customer && x.Deflag == true);

            return count;
        }

        public async Task<int> GetCustomerAccountsCreatedTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            var accountRepository = _unitOfWork.GetRepository<Account>();

            int count = await accountRepository.CountAsync(
                x => x.Role.RoleName == AppConstant.Role.Customer &&
                     x.InsDate.HasValue &&
                     x.InsDate.Value.Date == today
            );
            return count;
        }
    }
}
