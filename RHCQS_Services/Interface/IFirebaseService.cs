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
        Task SaveDeviceTokenAsync(Guid userId, string deviceToken);
        Task<string> GetDeviceTokenAsync(Guid userId);
        Task<string> SendNotificationAsync(Guid userId, string deviceToken, string title, string body);
        Task SaveNotificationAsync(Guid userId,string deviceToken, string title, string body);
        Task<List<NotificationResponse>> GetNotificationsAsync(Guid userId);
    }
}
