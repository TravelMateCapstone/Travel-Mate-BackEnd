using BussinessObjects;
using BussinessObjects.Entities;

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
            var notification = new BussinessObjects.Entities.Notification
            {
                UserId = userId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }

}
