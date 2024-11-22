using BusinessObjects.Entities;

namespace Repository.Interfaces
{
    public interface IPastTripPostRepository
    {
        Task<IEnumerable<PastTripPost>> GetAllAsync();
        Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId);

        Task<PastTripPost?> GetByIdAsync(int id);
        Task AddAsync(PastTripPost post);
        Task UpdateTravelerPartAsync(PastTripPost post);

        Task UpdateLocalPartAsync(PastTripPost post);
        Task DeleteAsync(int id);
    }
}
