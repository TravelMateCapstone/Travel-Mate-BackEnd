using BusinessObjects.Entities;

namespace Repository.Interfaces
{
    public interface IPastTripPostRepository
    {
        Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId);
        Task<PastTripPost?> GetPostByIdAsync(string id);
        Task AddAsync(PastTripPost post);
        Task UpdatePostAsync(string postId, PastTripPost post);
        Task DeleteAsync(string postId);
        Task<double> GetUserAverageStar(int userId);
        Task<int> GetUserTotalTrip(int userId);
    }
}
