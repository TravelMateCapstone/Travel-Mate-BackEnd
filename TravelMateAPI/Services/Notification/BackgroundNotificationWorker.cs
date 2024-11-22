using TravelMateAPI.Services.Notification.Event;

namespace TravelMateAPI.Services.Notification
{
    public class BackgroundNotificationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BackgroundNotificationWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<EventNotificationService>();
                    await notificationService.NotifyParticipantsForStartingEventsAsync();
                }

                // Chạy lại sau mỗi phút
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

}
