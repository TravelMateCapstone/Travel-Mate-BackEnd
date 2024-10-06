namespace TravelMateAPI.Services.Notification
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string message);
    }
}
