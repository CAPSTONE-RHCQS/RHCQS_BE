using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Implement;
using RHCQS_Services.Interface;
using System.Collections.Generic;
using System.Security.Claims;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        public AccountController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
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
        /*        [Authorize(Roles = "Customer")]*/
        [HttpGet(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAccountAsync(int page, int size)
        {
            var list = await _accountService.GetListAccountAsync(page, size);
            var accounts = JsonConvert.SerializeObject(list, Formatting.Indented);
            return Ok(accounts);
        }
        #region GetAccountsByRoleId
        /// <summary>
        /// Get a list of accounts by RoleId.
        /// </summary>
        /// <param name="roleId">The Role ID to filter accounts.</param>
        /// <returns>List of accounts with the specified Role ID.</returns>
        // GET: api/account/role/{roleId}
        #endregion
        [HttpGet(ApiEndPointConstant.Account.AccountByRoleIdEndpoint)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAccountByRoleIdAsync(Guid id, int page, int size)
        {
            var list = await _accountService.GetListAccountByRoleIdAsync(id,page, size);
            var accounts = JsonConvert.SerializeObject(list, Formatting.Indented);
            return Ok(accounts);
        }
        #region AccountProfile
        /// <summary>
        /// Profile account.
        /// </summary>
        /// <param name="profile">The account object with the data.</param>
        /// <returns>Profile.</returns>
        #endregion
        [Authorize]
        [HttpGet(ApiEndPointConstant.Account.AccountProfileEndpoint)]
        public async Task<IActionResult> GetAccountProfile()
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var accountProfile = await _accountService.GetAccountByIdAsync(Guid.Parse(accountId));
            return Ok(accountProfile);
        }
        #region GetActiveAccount
        /// <summary>
        /// Get active accounts.
        /// </summary>
        /// <returns>List of accounts.</returns>
        // GET: api/Account
        #endregion
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
        [HttpGet(ApiEndPointConstant.Account.AccountByIdEndpoint)]
        public async Task<ActionResult<Account>> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            return Ok(account);
        }

        #region GetTotalAccount
        /// <summary>
        /// Get Total accounts.
        /// </summary>
        /// <returns>List of accounts.</returns>
        // GET: api/Account
        #endregion
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
            return Ok(searchEmployee);
        }
        #region UpdateAccount
        /// <summary>
        /// Update an account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="account">The account object with updated data.</param>
        /// <returns>The updated account object.</returns>
        // PUT: api/account/{id}
        #endregion
        [HttpPut(ApiEndPointConstant.Account.AccountByIdEndpoint)]
        public async Task<ActionResult<Account>> UpdateAccountAsync(Guid id, [FromBody] AccountRequest accountRequest)
        {

            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound("Account not found.");
            }

            if (accountRequest.RoleId != Guid.Empty && existingAccount.RoleId != accountRequest.RoleId)
            {
                var role = await _roleService.GetRoleByIdAsync(accountRequest.RoleId);
                if (role == null)
                {
                    accountRequest.RoleId = existingAccount.RoleId;
                }
            }

            var account = new Account
            {
                Username = accountRequest.Username,
                PhoneNumber = accountRequest.PhoneNumber,
                DateOfBirth = accountRequest.DateOfBirth,
                PasswordHash = accountRequest.PasswordHash,
                ImageUrl = accountRequest.ImageUrl,
                Deflag = accountRequest.Deflag,
                RoleId = accountRequest.RoleId,
                InsDate = accountRequest.InsDate,
                UpsDate = accountRequest.UpsDate
            };

            var updatedAccount = await _accountService.UpdateAccountAsync(id, account);
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
        #region UpdateDeflagAccount
        /// <summary>
        /// Update deflag of an account by ID or ban account.
        /// </summary>
        /// <param name="id">The ID of the account to update deflag.</param>
        /// <returns>The updated account object with deflag set to false and status set to inactive.</returns>
        // PUT: api/Account/UpdateDeflag/{id}
        #endregion
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
    }
}
