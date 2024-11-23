using Microsoft.AspNetCore.Mvc;
using RHCQS_BE.Extenstion;
using RHCQS_BusinessObject.Payload.Request;
using RHCQS_Services.Interface;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using RHCQS_BusinessObjects;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;

        public NotificationController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        #region SendNotification
        /// <summary>
        /// Sends a notification to a mobile device.
        /// 
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
            if (string.IsNullOrEmpty(request.DeviceToken) || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
            {
                var errorResponse = JsonConvert.SerializeObject(new { Message = "DeviceToken, Title, Body, and UserId are required" });
                return Content(errorResponse, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                var response = await _firebaseService.SendNotificationAsync(request.Email, request.DeviceToken, request.Title, request.Body);

                var successResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.SUCCESSFUL_NOTIFICATION_SEND, Response = response });
                return Content(successResponse, "application/json", System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var errorResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.ERROR_NOTIFICATION_SEND, Error = ex.Message });
                return Content(errorResponse, "application/json", System.Text.Encoding.UTF8);
            }
        }

        #region GetNotifications
        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// ```json
        /// {
        ///   "userId": "guid of user"
        /// }
        /// ``` 
        /// </remarks>
        /// <param name="userId">The ID of the user whose notifications are to be retrieved</param>
        /// <returns>List of notifications</returns>
        /// <response code="200">Notifications retrieved successfully</response>
        /// <response code="404">No notifications found for the given user</response>
        /// <response code="500">Failed to retrieve notifications</response>
        #endregion
        [HttpGet(ApiEndPointConstant.Notification.GetNotificationsEndpoint)]
        public async Task<IActionResult> GetNotifications(string email)
        {
            try
            {
                var notifications = await _firebaseService.GetNotificationsAsync(email);

                if (notifications == null || notifications.Count == 0)
                {
                    var notFoundResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.NO_NOTIFICATIONS_FOUND });
                    return Content(notFoundResponse, "application/json", System.Text.Encoding.UTF8);
                }

                var successResponse = JsonConvert.SerializeObject(new { Notifications = notifications });
                return Content(successResponse, "application/json", System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var errorResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.ERROR_NOTIFICATION_SEND, Error = ex.Message });
                return Content(errorResponse, "application/json", System.Text.Encoding.UTF8);
            }
        }

        #region SaveDeviceToken
        /// <summary>
        /// Saves the device token for a user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// ```json
        /// {
        ///   "deviceToken": "FCM Token for the user",
        ///   "userId": "guid of the user"
        /// }
        /// ``` 
        /// </remarks>
        /// <param name="request">The request object containing the device token and user ID</param>
        /// <returns>Returns the result of saving the device token</returns>
        /// <response code="200">Device token saved successfully</response>
        /// <response code="400">Invalid request data or failed to save device token</response>
        /// <response code="500">Failed to save device token</response>
        #endregion
        [HttpPost(ApiEndPointConstant.Notification.SaveDeviceTokenEndpoint)]
        public async Task<IActionResult> SaveDeviceToken([FromBody] DeviceTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.DeviceToken))
            {
                var errorResponse = JsonConvert.SerializeObject(new { Message = "DeviceToken and UserId are required" });
                return Content(errorResponse, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                await _firebaseService.SaveDeviceTokenAsync(request.Email, request.DeviceToken);
                var successResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.SUCCESSFUL_DEVICE_TOKEN_SAVE });
                return Content(successResponse, "application/json", System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var errorResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.ERROR_DEVICE_TOKEN_SAVE, Error = ex.Message });
                return Content(errorResponse, "application/json", System.Text.Encoding.UTF8);
            }
        }

        #region GetDeviceToken
        /// <summary>
        /// Retrieves the device token for a specific user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// ```json
        /// {
        ///   "userId": "guid of the user"
        /// }
        /// ``` 
        /// </remarks>
        /// <param name="userId">The ID of the user whose device token is to be retrieved</param>
        /// <returns>The device token for the user</returns>
        /// <response code="200">Device token retrieved successfully</response>
        /// <response code="404">Device token not found for the given user</response>
        /// <response code="500">Failed to retrieve device token</response>
        #endregion
        [HttpGet(ApiEndPointConstant.Notification.GetDeviceTokenEndpoint)]
        public async Task<IActionResult> GetDeviceToken(string email)
        {
            try
            {
                var deviceToken = await _firebaseService.GetDeviceTokenAsync(email);
                if (deviceToken == null)
                {
                    var notFoundResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.ERROR_DEVICE_TOKEN_RETRIEVE });
                    return Content(notFoundResponse, "application/json", System.Text.Encoding.UTF8);
                }

                var successResponse = JsonConvert.SerializeObject(new { DeviceToken = deviceToken });
                return Content(successResponse, "application/json", System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                var errorResponse = JsonConvert.SerializeObject(new { Message = AppConstant.Message.ERROR_DEVICE_TOKEN_RETRIEVE, Error = ex.Message });
                return Content(errorResponse, "application/json", System.Text.Encoding.UTF8);
            }
        }
    }
}
