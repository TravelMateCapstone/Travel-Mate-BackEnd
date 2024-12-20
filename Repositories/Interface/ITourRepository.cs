using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects.Utils.Request;

namespace Repositories.Interface
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAllTours();
        Task<IEnumerable<Tour>> GetAllToursOfLocal(int userId);
        Task<IEnumerable<Tour>> GetToursByStatus(int userId, ApprovalStatus? approvalStatus);
        Task<IEnumerable<TourBriefDto>> GetTourBriefByUserId(int creatorId);
        Task<IEnumerable<Participants>> GetListParticipantsAsync(string tourId);
        Task<Tour> GetTourById(string id);
        Task AddTour(int userId, Tour tour);
        Task UpdateTour(string id, Tour tour);
        Task DeleteTour(string id);
        Task JoinTour(string tourId, int travelerId);
        Task RemoveUnpaidParticipantsAsync(string tourId, int travelerId);
        Task AcceptTour(string tourId);
        Task RejectTour(string tourId);
        Task CancelTour(string tourId);
        Task<ApplicationUser> GetUserInfo(int userId);
        Task<bool> DoesParticipantExist(string tourId, int userId);
        Task UpdatePaymentStatus(long orderCode, int totalAmount);
        Task UpdateOrderCode(string tourId, int travelerId, long orderCode);
        Task<bool> DidParticipantPay(long orderCode);
        Task<Tour> GetParticipantWithOrderCode(long orderCode);
        Task<DateTime> GetParticipantJoinTimeAsync(string tourId, int travelerId);
    }
}
