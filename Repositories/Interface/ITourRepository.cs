using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAllTours();
        Task<IEnumerable<Tour>> GetAllToursOfLocal(int userId);
        Task<IEnumerable<Tour>> GetToursByStatus(int userId, bool? approvalStatus);
        Task<Tour> GetTourById(string id);
        Task AddTour(int userId, Tour tour);
        Task UpdateTour(string id, Tour tour);
        Task DeleteTour(string id);
        Task JoinTour(string tourId, Participants participant);
        Task AcceptTour(string tourId);
        Task AddReview(string tourId, TourReview tourReview);
        Task UpdateAvailability(string tourId, int slots);
        Task CancelTour(string tourId);
    }
}
