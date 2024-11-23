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
        #endregion

        #region GetNotifications
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
        #endregion

        #region SaveDeviceToken
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
        #endregion

        #region GetDeviceToken
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
        #endregion
    }
}
