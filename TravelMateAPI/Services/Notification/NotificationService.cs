using BusinessObjects;
using BusinessObjects.Entities;

namespace TravelMateAPI.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDBContext _context;

        public NotificationService(ApplicationDBContext context)
        {
            _context = context;
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
    }

}
