namespace TravelMateAPI.Services.Notification
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string message);
        Task CreateNotificationFullAsync(int userId, string message , int senderId , int typeNotificaton);
    }
}
