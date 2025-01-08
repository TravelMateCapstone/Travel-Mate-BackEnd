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
        Task<Tour> GetTourById(string id);
        Task AddTour(int userId, Tour tour);
        Task UpdateTour(string id, Tour tour);
        Task DeleteTour(string id);
        Task AcceptTour(string tourId);
        Task RejectTour(string tourId);
        Task CancelTour(string tourId);
        Task<ApplicationUser> GetUserInfo(int userId);

        Task UpdateOrderCode(string scheduleId, string tourId, int travelerId, long orderCode);
        Task<IEnumerable<ApplicationUser>> GetUsersInfoAsync(IEnumerable<int> userIds);
    }
}
