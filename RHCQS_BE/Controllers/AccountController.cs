using CloudinaryDotNet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static RHCQS_BusinessObjects.AppConstant;
using Account = RHCQS_DataAccessObjects.Models.Account;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IUploadImgService _uploadImgService;
        public AccountController(IAccountService accountService, IRoleService roleService, IUploadImgService uploadImgService)
        {
            _accountService = accountService;
            _roleService = roleService;
            _uploadImgService = uploadImgService;
        }
        #region Register Staff API
        /// <summary>
        /// Manager create staff account.
        /// </summary>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPost(ApiEndPointConstant.Account.CreateStaffEndpoint)]
        public async Task<IActionResult> RegisterStaff([FromBody] RegisterRequest registerRequest,
            [FromQuery][Required(ErrorMessage = "Role là cần thiết.")] UserRoleForManagerRegister role)
        {
            if (!Enum.IsDefined(typeof(UserRoleForManagerRegister), role))
            {
                return BadRequest(new
                {
                    message = $"Role '{role}' không hợp lệ. Vai trò hợp lệ là: {string.Join(", ", Enum.GetNames(typeof(UserRoleForManagerRegister)))}"
                });
            }
            var result = await _accountService.RegisterForStaffAsync(registerRequest, role);
            var response = JsonConvert.SerializeObject(new { message = "Đăng kí thành công!" }, Formatting.Indented);

            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region GetAccount
        /// <summary>
        /// Get a paginated list of accounts.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="size">The number of accounts per page.</param>
        /// <returns>Returns an HTTP 200 response with a list of accounts in JSON format.</returns>
        /// <response code="200">Returns the list of accounts.</response>
        /// <response code="400">If the request parameters are invalid (e.g., page or size is less than 1).</response>
        /// <response code="401">If the user is not authorized to access this resource.</response>
        /// <response code="404">If no accounts are found.</response>
        /// <response code="500">If there is an internal server error.</response>
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpGet(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAccountAsync(int page, int size)
        {
            var list = await _accountService.GetListAccountAsync(page, size);
            var accounts = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult
            {
                Content = accounts,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region GetAccountsByRoleId
        /// <summary>
        /// Get a list of accounts by RoleId.
        /// </summary>
        /// <param name="id">The Role ID to filter accounts.</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns>List of accounts with the specified Role ID.</returns>
        // GET: api/account/role/{roleId}
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.AccountByRoleIdEndpoint)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAccountByRoleIdAsync(Guid id, int page, int size)
        {
            var list = await _accountService.GetListAccountByRoleIdAsync(id, page, size);
            var accounts = JsonConvert.SerializeObject(list, Formatting.Indented);
            return new ContentResult
            {
                Content = accounts,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region AccountProfile
        /// <summary>
        /// Profile account.
        /// </summary>
        /// <returns>Profile.</returns>
        #endregion
        [Authorize]
        [HttpGet(ApiEndPointConstant.Account.AccountProfileEndpoint)]
        public async Task<IActionResult> GetAccountProfile()
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var accountProfile = await _accountService.GetAccountByIdAsync(Guid.Parse(accountId!));

            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var profile = JsonConvert.SerializeObject(accountProfile, settings);

            return new ContentResult
            {
                Content = profile,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region GetActiveAccount
        /// <summary>
        /// Get active accounts.
        /// </summary>
        /// <returns>List of accounts.</returns>
        // GET: api/Account
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.ActiveAccountEndpoint)]
        public async Task<ActionResult<int>> GetActiveAccountCount()
        {
            var activeAccountCount = await _accountService.GetActiveAccountCountAsync();
            return Ok(activeAccountCount);
        }
        #region GetAccountById
        /// <summary>
        /// Get a account by ID.
        /// </summary>
        /// <param name="id">The ID of the stall to retrieve.</param>
        /// <returns>The stall with the specified ID.</returns>
        // GET: api/account/{id}
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.AccountByIdEndpoint)]
        public async Task<ActionResult<Account>> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var acount = JsonConvert.SerializeObject(account, settings);

            return new ContentResult
            {
                Content = acount,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }

        #region GetTotalAccount
        /// <summary>
        /// Get Total accounts.
        /// </summary>
        /// <returns>List of accounts.</returns>
        // GET: api/Account
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.TotalAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalAccountCount()
        {
            var totalAccountCount = await _accountService.GetTotalAccountCountAsync();
            return Ok(totalAccountCount);
        }
        #region SearchAccounts
        /// <summary>
        /// Search listaccount by name or phonenumber.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <param phone="phone">The phone to search for.</param>
        /// <returns>The account that match the search criteria.</returns>
        // GET: api/Account/Search
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.SearchAccountByPhoneOrNameEndpoint)]
        public async Task<ActionResult> SearchAccountsByKeyAsync(string searchKey, int page, int size)
        {

            var accounts = await _accountService.SearchAccountsByKeyAsync(searchKey, page, size);

            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var serializedResult = JsonConvert.SerializeObject(accounts, settings);

            return new ContentResult
            {
                Content = serializedResult,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };

        }
        #region SearchAccountsByname
        /// <summary>
        /// Search account by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The account that match the search criteria.</returns>
        // GET: api/Account/Search
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.SearchAccountEndpoint)]
        public async Task<ActionResult<Account>> SearchAccountsByNameAsync(string searchkey)
        {
            var account = await _accountService.SearchAccountsByNameAsync(searchkey);

            var searchEmployee = new Account
            {
                Id = account.Id,
                Email = account.Email,
                Username = account.Username,
                PhoneNumber = account.PhoneNumber,
                DateOfBirth = account.DateOfBirth,
                ImageUrl = account.ImageUrl,
                Deflag = account.Deflag,
                RoleId = account.RoleId,
                InsDate = account.InsDate,
                UpsDate = account.UpsDate,
                Role = account.Role
            };
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var acount = JsonConvert.SerializeObject(searchEmployee, settings);

            return new ContentResult
            {
                Content = acount,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region UpdateAccount
        /// <summary>
        /// Update an account by ID.
        /// </summary>
        /// <remarks>
        /// Sample JSON input for updating an account:
        /// ```json
        /// {
        ///   "username": "Ngân test",
        ///   "imageUrl": "https://chungkhoantaichinh.vn/wp-content/uploads/2022/12/avatar-meo-cute-de-thuong-05.jpg",
        ///   "phoneNumber": "0906697051",
        ///   "dateOfBirth": "2002-01-01" (YYYY-MM-DD)
        /// }
        /// ```
        /// </remarks>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="accountRequest">The account object with updated data.</param>
        /// <returns>The updated account object.</returns>
        // PUT: api/account/{id}
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpPut(ApiEndPointConstant.Account.AccountByIdEndpoint)]
        public async Task<ActionResult<Account>> UpdateAccountAsync(Guid id, [FromBody] AccountRequestForUpdate accountRequest)
        {

            var updatedAccount = await _accountService.UpdateAccountAsync(id, accountRequest);
            if (updatedAccount == null)
            {
                return StatusCode(500, "An error occurred while updating the account.");
            }
            var accountResponse = new AccountResponse(
                updatedAccount.Id,
                updatedAccount.Username,
                updatedAccount.PhoneNumber,
                updatedAccount.DateOfBirth,
                updatedAccount.PasswordHash,
                updatedAccount.Email,
                updatedAccount.ImageUrl,
                updatedAccount.Deflag,
                updatedAccount.Role.RoleName,
                updatedAccount.RoleId,
                updatedAccount.InsDate,
                updatedAccount.UpsDate
            );

            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var returnAccount = JsonConvert.SerializeObject(accountResponse, settings);

            return new ContentResult
            {
                Content = returnAccount,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region UpdateProfile
        /// <summary>
        /// Update a profile by ID for Customer.
        /// </summary>
        /// <remarks>
        /// Sample JSON input for updating an account:
        /// ```json
        /// {
        ///   "username": "Ngân test",
        ///   "imageUrl": "https://chungkhoantaichinh.vn/wp-content/uploads/2022/12/avatar-meo-cute-de-thuong-05.jpg",
        ///   "phoneNumber": "0906697051",
        ///   "dateOfBirth": "2002-01-01" (YYYY-MM-DD)
        /// }
        /// ```
        /// </remarks>
        /// <returns>The updated profile object.</returns>
        // PUT: api/account/profile/{id}
        #endregion
        [Authorize(Roles = "Customer")]
        [HttpPut(ApiEndPointConstant.Account.ProfileEndpoint)]
        public async Task<ActionResult<Account>> UpdateProfileForCustomerAsync([FromBody] AccountRequestForUpdateProfile accountRequest)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingAccount = await _accountService.GetAccountByIdAsync(Guid.Parse(accountId!));

            var updatedAccount = await _accountService.UpdateProfileAsync(existingAccount.Id, accountRequest);
            if (updatedAccount == null)
            {
                return StatusCode(500, "An error occurred while updating the account.");
            }

            var accountResponse = new AccountResponse(
                updatedAccount.Id,
                updatedAccount.Username,
                updatedAccount.PhoneNumber,
                updatedAccount.DateOfBirth,
                updatedAccount.PasswordHash,
                updatedAccount.Email,
                updatedAccount.ImageUrl,
                updatedAccount.Deflag,
                updatedAccount.Role.RoleName,
                updatedAccount.RoleId,
                updatedAccount.InsDate,
                updatedAccount.UpsDate
            );

            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var returnAccount = JsonConvert.SerializeObject(accountResponse, settings);

            return new ContentResult
            {
                Content = returnAccount,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region UpdatePassword
        /// <summary>
        /// Update the password of the currently authenticated user.
        /// </summary>
        /// <param name="passwordUpdateRequest">The current and new password for the update.</param>
        /// <returns>HTTP 200 if successful, otherwise appropriate error codes.</returns>
        #endregion
        [Authorize]
        [HttpPut(ApiEndPointConstant.Account.UpdatePasswordEndpoint)]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] PasswordUpdateRequest passwordUpdateRequest)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var account = await _accountService.GetAccountByIdAsync(Guid.Parse(accountId));

            var result = await _accountService.UpdatePasswordAsync(account.Id, passwordUpdateRequest.CurrentPassword, passwordUpdateRequest.NewPassword);

            return Ok(result ? AppConstant.Message.PASSWORD_SUCESSFUL : AppConstant.Message.ERROR);
        }

        #region UpdateDeflagAccount
        /// <summary>
        /// Update deflag of an account by ID or ban account.
        /// </summary>
        /// <param name="id">The ID of the account to update deflag.</param>
        /// <returns>The updated account object with deflag set to false and status set to inactive.</returns>
        // PUT: api/Account/UpdateDeflag/{id}
        #endregion
        [Authorize(Roles = "Manager")]
        [HttpPut(ApiEndPointConstant.Account.UpdateDeflagEndpoint)]
        public async Task<ActionResult<AccountResponse>> UpdateDeflagAccountAsync(Guid id)
        {
            var updatedAccount = await _accountService.UpdateDeflagAccountAsync(id);


            var account = new AccountResponse(
                updatedAccount.Id,
                updatedAccount.Username,
                updatedAccount.PhoneNumber,
                updatedAccount.DateOfBirth,
                updatedAccount.PasswordHash,
                updatedAccount.Email,
                updatedAccount.ImageUrl,
                updatedAccount.Deflag,
                updatedAccount.Role.RoleName,
                updatedAccount.RoleId,
                updatedAccount.InsDate,
                updatedAccount.UpsDate
            );
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var returnAccount = JsonConvert.SerializeObject(account, settings);

            return new ContentResult
            {
                Content = returnAccount,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #region UpdateImgforAccount
        /// <summary>
        /// Update image for account.
        /// </summary>
        /// <returns>The updated img account.</returns>
        #endregion
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.Account.UploadImageProfileEndpoint)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImageForAccount([FromForm] ImageForAccount request)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (request.AccountImage == null)
            {
                return BadRequest("Phải có hình hình.");
            }
            var isUploaded = await _accountService.CreateImageAccount(Guid.Parse(accountId!), request);
            if (isUploaded != null)
            {
                var returntoken = JsonConvert.SerializeObject(isUploaded, Formatting.Indented);

                return new ContentResult
                {
                    Content = returntoken,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                return BadRequest("Cập nhật ảnh thất bại");
            }

        }

        #region GetTotalSStaffAccount
        /// <summary>
        /// Get total sales staff accounts.
        /// </summary>
        /// <returns>Number of sales staff accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.TotalSStaffAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalSStaffAccountCount()
        {
            var totalSStaffAccountCount = await _accountService.GetSStaffAccountCountAsync();
            return Ok(totalSStaffAccountCount);
        }

        #region GetTotalDStaffAccount
        /// <summary>
        /// Get total design staff accounts.
        /// </summary>
        /// <returns>Number of design staff accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.TotalDStaffAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalDStaffAccountCount()
        {
            var totalDStaffAccountCount = await _accountService.GetDStaffAccountCountAsync();
            return Ok(totalDStaffAccountCount);
        }

        #region GetTotalCustomerAccount
        /// <summary>
        /// Get total customer accounts.
        /// </summary>
        /// <returns>Number of customer accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.TotalCustomerAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalCustomerAccount()
        {
            var totalCustomerAccountCount = await _accountService.GetCustomerAccountCountAsync();
            return Ok(totalCustomerAccountCount);
        }

        #region GetTotalCustomerAccountToday
        /// <summary>
        /// Get total customer accounts today.
        /// </summary>
        /// <returns>Number of customer accounts.</returns>
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.TotalCustomerTodayAccountEndpoint)]
        public async Task<ActionResult<int>> GetTotalCustomerAccountToday()
        {
            var totalCustomerAccountCount = await _accountService.GetCustomerAccountsCreatedTodayAsync();
            return Ok(totalCustomerAccountCount);
        }
    }
}
