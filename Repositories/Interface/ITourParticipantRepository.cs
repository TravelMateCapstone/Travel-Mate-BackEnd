using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITourParticipantRepository
    {
        Task<IEnumerable<TravelerTransaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Participants>> GetListParticipantsAsync(string scheduleId, string tourId);
        Task JoinTour(string scheduleId, string tourId, int travelerId);
        Task<ApplicationUser> GetUserInfo(int userId);
        Task<Tour> GetTourScheduleById(string scheduleId, string tourId);
        Task RemoveUnpaidParticipantsAsync(string scheduleId, string tourId, int travelerId);
        Task UpdatePaymentStatus(Tour tour, int travelerId);
        Task UpdateRefundDone(string tourId, string scheduleId, int userId);

        Task UpdateRefundStatus(Tour tour, string scheduleId, int userId);

        Task<Tour> GetParticipantWithOrderCode(long orderCode);

        Task ProcessTourStatus(string scheduleId, string tourId, bool isActive);

        Task<IEnumerable<TravelerTransaction>> GetTransactionList(int travelerId);

        Task AddTransactionAsync(TravelerTransaction transaction);
    }
}
