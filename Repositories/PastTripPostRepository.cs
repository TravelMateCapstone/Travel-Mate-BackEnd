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


    }
}
