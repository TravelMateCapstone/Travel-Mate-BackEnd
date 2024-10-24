using BusinessObjects.Entities;

namespace TravelMateAPI.Services.FindLocal
{
    public interface IFindLocalService
    {
        //Task<List<ApplicationUser>> SearchLocalsWithMatchingActivities(int travelerId, int locationId);
        Task<List<ApplicationUser>> GetMatchingUsersAsync(int locationId, List<int> activityIds, string roleName, int pageNumber, int pageSize);
    }
}
