using RHCQS_BusinessObject.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHCQS_Services.Interface
{
    public interface IFirebaseService
    {
        Task SaveDeviceTokenAsync(string email, string deviceToken);
        Task<string> GetDeviceTokenAsync(string email);
        Task<string> SendNotificationAsync(string email, string deviceToken, string title, string body);
        Task SaveNotificationAsync(string email, string deviceToken, string title, string body);
        Task<List<NotificationResponse>> GetNotificationsAsync(string email);
    }
}
