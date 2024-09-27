using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }
        #region Login
        /// <summary>
        /// Login JWT
        /// </summary>
        /// <returns>Home</returns> 
        /// GET : api/Login
        #endregion
        [AllowAnonymous]
        [HttpPost(ApiEndPointConstant.Auth.LoginEndpoint)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null)
                    return BadRequest("Invalid client request");

                var user = await _authService.GetAccountByEmail(loginRequest.Email, loginRequest.Password);
                var token = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);
                return Ok(JsonConvert.SerializeObject(new LoginResponse(token), Formatting.Indented));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        #region Logout
        /// <summary>
        /// Logs the user out of the application.
        /// </summary>
        /// <returns>Returns a message indicating a successful logout.</returns>
        /// <remarks>
        /// This action clears the user's session or authentication tokens, effectively logging the user out of the system.
        /// </remarks>
        /// <response code="200">Returns a success message if the logout is successful.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// POST: api/Logout
        #endregion
        [HttpPost(ApiEndPointConstant.Auth.LogoutEndpoint)]
        public IActionResult Logout()
        {

            return Ok(new { message = "Logout successful" });
        }
    }
}
