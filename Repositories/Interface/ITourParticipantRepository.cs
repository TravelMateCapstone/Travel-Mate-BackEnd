using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITourParticipantRepository
    {
        Task<IEnumerable<Participants>> GetListParticipantsAsync(string scheduleId, string tourId);
        Task JoinTour(string scheduleId, string tourId, int travelerId);
        Task<ApplicationUser> GetUserInfo(int userId);
        Task<Tour> GetTourScheduleById(string scheduleId, string tourId);
        Task RemoveUnpaidParticipantsAsync(string scheduleId, string tourId, int travelerId);
        Task UpdatePaymentStatus(Tour tour, int travelerId);
        Task<Tour> GetParticipantWithOrderCode(long orderCode);
    }
}
