using BusinessObjects;
using BusinessObjects.Entities;
using MongoDB.Driver;

namespace DataAccess
{
    public class PastTripPostDAO
    {
        private readonly ApplicationDBContext _context;
        private readonly IMongoCollection<PastTripPost> _mongoContext;
        public PastTripPostDAO(ApplicationDBContext context, MongoDbContext mongoContext)
        {
            _context = context;
            _mongoContext = mongoContext.GetCollection<PastTripPost>("PastTripPosts");
        }

    }
}
