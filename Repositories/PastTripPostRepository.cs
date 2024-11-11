using BusinessObjects.Entities;
using DataAccess;
using Repository.Interfaces;

namespace Repositories
{
    public class PastTripPostRepository : IPastTripPostRepository
    {
        private readonly PastTripPostDAO _pastTripPostDAO;

        public PastTripPostRepository(PastTripPostDAO pastTripPostDAO)
        {
            _pastTripPostDAO = pastTripPostDAO;
        }

        public async Task<IEnumerable<PastTripPost>> GetAllAsync()
        {
            return await _pastTripPostDAO.GetAllPostAsync();
        }
        public async Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId)
        {
            return await _pastTripPostDAO.GetAllPostOfUserAsync(userId);
        }

        public async Task<PastTripPost?> GetByIdAsync(int id)
        {
            return await _pastTripPostDAO.GetByIdAsync(id);
        }

        public async Task AddAsync(PastTripPost post)
        {
            // Here you could add any additional logic or validation before adding
            await _pastTripPostDAO.AddAsync(post);
        }

        public async Task DeleteAsync(int id)
        {
            await _pastTripPostDAO.DeleteAsync(id);
        }

        public async Task UpdateTravelerPartAsync(PastTripPost post)
        {
            await _pastTripPostDAO.UpdateTravelerPartAsync(post);
        }

        public async Task UpdateLocalPartAsync(PastTripPost post)
        {
            await _pastTripPostDAO.UpdateLocalPartAsync(post);
        }
    }
}
