using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.SignalR;
using TravelMateAPI.Services.Hubs;

namespace TravelMateAPI.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHubContext<ServiceHub> _hubContext;

        public NotificationService(ApplicationDBContext context, IHubContext<ServiceHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task CreateNotificationAsync(int userId, string message)
        {
            var notification = new BusinessObjects.Entities.Notification
            {
                UserId = userId,
                Message = message,
                IsRead = false,
                CreatedAt = GetTimeZone.GetVNTimeZoneNow()
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNotificationFullAsync(int userId, string message, int senderId, int typeNotification)
        {
            var notification = new BusinessObjects.Entities.Notification
            {
                UserId = userId,
                Message = message,
                SenderId = senderId,
                TypeNotification = typeNotification,
                IsRead = false,
                CreatedAt = GetTimeZone.GetVNTimeZoneNow()
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.SendAsync("NotificationCreated", notification);  // Gửi sự kiện NotificationCreated đến tất cả client
            // Gửi sự kiện NotificationCreated chỉ đến client có userId tương ứng
            await _hubContext.Clients.User(userId.ToString()).SendAsync("NotificationCreated", notification);
        }
    }

}
