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

        public Task AddAsync(PastTripPost post)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PastTripPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PastTripPost?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLocalPartAsync(PastTripPost post)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTravelerPartAsync(PastTripPost post)
        {
            throw new NotImplementedException();
        }
    }
}
