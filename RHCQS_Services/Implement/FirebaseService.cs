﻿using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json.Linq;
using RHCQS_BusinessObject.Helper;
using RHCQS_BusinessObject.Payload.Response;
using RHCQS_Services.Interface;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Notification = FirebaseAdmin.Messaging.Notification;
using Microsoft.Extensions.Configuration;

namespace RHCQS_Services.Implement
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly FirebaseMessaging _firebaseMessaging;

        public FirebaseService(IConfiguration configuration)
        {
            var firebaseDatabaseUrl = configuration["Firebase:DatabaseUrl"];

            _firebaseClient = new FirebaseClient(firebaseDatabaseUrl);
            _firebaseMessaging = FirebaseMessaging.DefaultInstance;
        }

        public async Task SaveDeviceTokenAsync(string email, string deviceToken)
        {
            try
            {
                var tokenData = new { Token = deviceToken };
                await _firebaseClient
                    .Child("deviceTokens")
                    .Child(email)
                    .PutAsync(tokenData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving device token: {ex.Message}");
                throw new Exception("Failed to save device token", ex);
            }
        }
        public async Task SaveNotificationAsync(string email, string deviceToken, string title, string body)
        {
            try
            {
                var notificationData = new
                {
                    DeviceToken = deviceToken,
                    Title = title,
                    Body = body,
                    Timestamp = LocalDateTime.VNDateTime().ToString("dd/MM/yyyy HH:mm:ss")
                };

                await _firebaseClient
                    .Child("notifications")
                    .Child(email)
                    .PostAsync(notificationData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving notification: {ex.Message}");
                throw new Exception("Failed to save notification", ex);
            }
        }

        public async Task<string> GetDeviceTokenAsync(string email)
        {
            try
            {
                var tokenData = await _firebaseClient
                    .Child("deviceTokens")
                    .Child(email)
                    .OnceSingleAsync<object>();

                if (tokenData != null)
                {
                    JObject jsonData = JObject.FromObject(tokenData);

                    string token = jsonData["Token"]?.ToString();

                    return token;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching device token: {ex.Message}");
                throw new Exception("Failed to get device token", ex);
            }
        }

        public async Task<string> SendNotificationAsync(string email, string deviceToken, string title, string body)
        {
            try
            {
                var message = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    }
                };

                var response = await _firebaseMessaging.SendAsync(message);

                await SaveNotificationAsync(email, deviceToken, title, body);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
                throw new Exception("Failed to send notification", ex);
            }
        }
        public async Task<List<NotificationResponse>> GetNotificationsAsync(string email)
        {
            try
            {
                var notificationsData = await _firebaseClient
                    .Child("notifications")
                    .Child(email)
                    .OnceAsync<NotificationResponse>(); // Fetch as NotificationModel directly

                // Convert the data to a list of NotificationModel objects
                return notificationsData
                    .Select(item => item.Object) // Access the NotificationModel object
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching notifications: {ex.Message}");
                throw new Exception("Failed to retrieve notifications", ex);
            }
        }

    }
}
