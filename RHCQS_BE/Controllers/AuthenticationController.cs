using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;
using static RHCQS_BusinessObjects.AppConstant;
using System.ComponentModel.DataAnnotations;
using RHCQS_BusinessObjects;

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
        /// GET : api/login
        #endregion
        [AllowAnonymous]
        [HttpPost(ApiEndPointConstant.Auth.LoginEndpoint)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);
            return Ok(JsonConvert.SerializeObject(new LoginResponse(token), Formatting.Indented));
        }
        #region Register
        /// <summary>
        /// Register account
        /// </summary>
        /// <returns>Home</returns> 
        /// GET : api/register
        #endregion
        [AllowAnonymous]
        [HttpPost(ApiEndPointConstant.Auth.RegisterEndpoint)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest,
            [FromQuery][Required(ErrorMessage = "Role is required.")] UserRoleForRegister role)
        {
            var result = await _authService.RegisterAsync(registerRequest, role);
            return Ok(JsonConvert.SerializeObject(new { message = "Đăng kí thành công!" }));
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
