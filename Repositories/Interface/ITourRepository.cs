using BusinessObjects.Entities;
using BusinessObjects.EnumClass;

namespace Repositories.Interface
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAllTours();
        Task<IEnumerable<Tour>> GetAllToursOfLocal(int userId);
        Task<IEnumerable<Tour>> GetToursByStatus(int userId, ApprovalStatus? approvalStatus);
        Task<Tour> GetTourById(string id);
        Task AddTour(int userId, Tour tour);
        Task UpdateTour(string id, Tour tour);
        Task DeleteTour(string id);
        Task JoinTour(string tourId, int travelerId);
        Task AcceptTour(string tourId);
        Task BanTour(string tourId);
        Task AddReview(string tourId, TourReview tourReview);
        Task UpdateAvailability(string tourId, int slots);
        Task CancelTour(string tourId);
        Task<ApplicationUser> GetUserInfo(int userId);

        Task<bool> DoesParticipantExist(int userId);
    }
}
