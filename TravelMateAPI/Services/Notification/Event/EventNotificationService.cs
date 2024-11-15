using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace TravelMateAPI.Services.Notification.Event
{
    public class EventNotificationService
    {
        private readonly ApplicationDBContext _context;
        private readonly INotificationService _notificationService;

        public EventNotificationService(ApplicationDBContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task NotifyParticipantsForStartingEventsAsync()
        {
            // Lấy ngày giờ hiện tại (theo múi giờ Việt Nam) +30 phút để báo trước
            var currentTime = GetTimeZone.GetVNTimeZoneNow().AddMinutes(30);

            // Lấy các sự kiện bắt đầu đúng vào thời điểm hiện tại
            var eventsStartingNow = await _context.Events
                .Where(e => e.StartAt.Date == currentTime.Date &&
                            e.StartAt.Hour == currentTime.Hour &&
                            e.StartAt.Minute == currentTime.Minute)
                .Include(e => e.EventParticipants) // Gộp thông tin người tham gia
                .ToListAsync();

            // Lặp qua từng sự kiện
            foreach (var eventDetails in eventsStartingNow)
            {
                // Gửi thông báo cho từng người tham gia sự kiện
                foreach (var participant in eventDetails.EventParticipants)
                {
                    if (participant.Notification == true) // Chỉ thông báo nếu người tham gia bật chế độ nhận thông báo
                    {
                        await _notificationService.CreateNotificationFullAsync(participant.UserId,
                        $"Sự kiện '{eventDetails.EventName}' sẽ bắt đầu sau 30 phút tại {eventDetails.EventLocation}!",0,1);
                    }
                }
            }
        }
    }

}
