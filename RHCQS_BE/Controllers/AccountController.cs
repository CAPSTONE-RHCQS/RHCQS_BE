using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
        [Authorize(Roles = "Customer")]
        [HttpGet(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAccountAsync(int page, int size)
        {
            // Validate input parameters
            if (page < 1 || size < 1)
            {
                return BadRequest("Page and size must be greater than 0.");
            }

            try
            {
                var accounts = await _accountService.GetListAccountAsync(page, size);
                if (accounts == null)
                {
                    return NotFound("No accounts found.");
                }

                return Ok(accounts);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

    }
}
