using CloudinaryDotNet;
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
using System.Security.Claims;
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
        /// Search account by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The account that match the search criteria.</returns>
        // GET: api/Account/Search
        #endregion
        [Authorize(Roles = "DesignStaff, SalesStaff, Manager")]
        [HttpGet(ApiEndPointConstant.Account.SearchAccountEndpoint)]
        public async Task<ActionResult<Account>> SearchAccountsByNameAsync(string name)
        {
            var account = await _accountService.SearchAccountsByNameAsync(name);

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

/*            var existingAccount = await _accountService.GetAccountByIdAsync(id);
 
            if (accountRequest.ImageUrl != null)
            {
                var imageUrl = await _uploadImgService.UploadImageAsync(accountRequest.ImageUrl, "profile");
                existingAccount.ImageUrl = imageUrl;
            }*/
/*            var account = new Account
            {
                Username = accountRequest.Username,
                PhoneNumber = accountRequest.PhoneNumber,
                DateOfBirth = accountRequest.DateOfBirth,
                ImageUrl = null,
                Deflag = accountRequest.Deflag,
            };*/

            var updatedAccount = await _accountService.UpdateAccountAsync(id, accountRequest);
            if (updatedAccount == null)
            {
                return StatusCode(500, "An error occurred while updating the account.");
            }

            return Ok(new AccountResponse(
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
            ));
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

/*            if (accountRequest.ImageUrl != null)
            {
                var imageUrl = await _uploadImgService.UploadImageAsync(accountRequest.ImageUrl, "profile");
                existingAccount.ImageUrl = imageUrl;
            }*/
/*            var account = new Account
            {
                Username = accountRequest.Username,
                PhoneNumber = accountRequest.PhoneNumber,
                DateOfBirth = accountRequest.DateOfBirth,
                ImageUrl = accountRequest.ImageUrl,
            };*/

            var updatedAccount = await _accountService.UpdateProfileAsync(existingAccount.Id, accountRequest);
            if (updatedAccount == null)
            {
                return StatusCode(500, "An error occurred while updating the account.");
            }

            return Ok(new AccountResponse(
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
            ));
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

            return Ok(new AccountResponse(
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
            ));
        }
        [Authorize(Roles = "Customer, DesignStaff, SalesStaff, Manager")]
        [HttpPost(ApiEndPointConstant.Account.UploadImageProfileEndpoint)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImageForAccount([FromForm] ImageForAccount request)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (request.AccountImage == null)
            {
                return BadRequest("At least one image file is required.");
            }

            try
            {
                var isUploaded = await _accountService.CreateImageAccount(Guid.Parse(accountId!), request);
                if (isUploaded)
                {
                    return Ok("Images uploaded successfully.");
                }
                else
                {
                    return BadRequest("Image upload failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error uploading images: {ex.Message}");
            }
        }
    }
}
