using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_BusinessObject.Payload.Response;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public NotificationController()
        {
            _firebaseMessaging = FirebaseMessaging.DefaultInstance;
        }

        #region SendNoti
        /// <summary>
        /// Sends a notification to a mobile device.
        /// 
        /// Role: USER, ADMIN
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// ```json
        /// {
        ///   "deviceToken": "..." {deviceToken(FCM Token) where the notification is sent to},
        ///   "title": "hello",
        ///   "body": "hello from there"
        /// }
        /// ```
        /// </remarks>
        /// <param name="request">The request object containing the mobile device details and notification content</param>
        /// <returns>Returns the result of the notification sending process</returns>
        /// <response code="200">Notification sent successfully</response>
        /// <response code="400">Invalid request data or notification sending failed</response>
        /// <response code="401">Unauthorized access</response>
        #endregion
        [HttpPost(ApiEndPointConstant.Notification.SendNotificationRoleEndpoint)]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            var message = new Message()
            {
                Token = request.DeviceToken,
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body
                }
            };

            try
            {
                string response = await _firebaseMessaging.SendAsync(message);
                return Ok(new { Message = "Notification sent successfully", Response = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to send notification", Error = ex.Message });
            }
        }
    }
}
