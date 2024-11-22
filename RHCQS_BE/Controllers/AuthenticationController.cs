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
using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth.OAuth2.Requests;
using RefreshTokenRequest = RHCQS_BusinessObject.Payload.Request.RefreshTokenRequest;

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
            var logintoken = new LoginResponse(token);
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var returntoken = JsonConvert.SerializeObject(logintoken, settings);

            return new ContentResult
            {
                Content = returntoken,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
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
            [FromQuery][Required(ErrorMessage = "Role là cần thiết.")] UserRoleForRegister role)
        {
            var result = await _authService.RegisterAsync(registerRequest, role);
            var response = JsonConvert.SerializeObject(new { message = "Đăng kí thành công!" }, Formatting.Indented);

            return new ContentResult
            {
                Content = response,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        [AllowAnonymous]
        [HttpPost(ApiEndPointConstant.Auth.RefreshTokenEndpoint)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.expiredTokenToken))
            {
                return BadRequest(new { message = "Token is required" });
            }

            try
            {
                var newToken = await _authService.RefreshTokenAsync(request.expiredTokenToken);
                var refreshToken = new LoginResponse(newToken);
                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var returntoken = JsonConvert.SerializeObject(refreshToken, settings);

                return new ContentResult
                {
                    Content = returntoken,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (AppConstant.MessageError ex)
            {
                var response = JsonConvert.SerializeObject(new { message = ex.Message }, Formatting.Indented);

                return new ContentResult
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = ex.Code
                };
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

        //[HttpPost("decode")]
        //public IActionResult DecodeToken([FromBody] TokenRequest request)
        //{
        //    if (string.IsNullOrEmpty(request.Token))
        //    {
        //        return BadRequest("Token is required.");
        //    }

        //    // Gọi hàm DecodeToken của AuthService để lấy thông tin
        //    var info = _authService.DecodeToken(request.Token);

        //    if (info == null)
        //    {
        //        return BadRequest("Invalid token.");
        //    }

        //    return Ok(info);
        //}
    }
}
