using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DataAccess
{
    public class TourDAO
    {
        private readonly ApplicationDBContext _sqlContext;
        private readonly IMongoCollection<Tour> _mongoContext;
        private readonly IMongoCollection<PastTripPost> _postMongoContext;

        public TourDAO(ApplicationDBContext context, MongoDbContext mongoContext)
        {
            _sqlContext = context;
            _mongoContext = mongoContext.GetCollection<Tour>("Tours");
            _postMongoContext = mongoContext.GetCollection<PastTripPost>("PastTripPosts");
        }

        public async Task<ApplicationUser> GetLocalInfor(int userId)
        {
            return _sqlContext.Users
                .Include(t => t.Profiles)
                .FirstOrDefault(t => t.Id == userId);
        }

        public async Task<ApplicationUser> GetUserInfor(int userId)
        {
            return _sqlContext.Users
                .Include(t => t.Profiles)
                .FirstOrDefault(t => t.Id == userId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInfoAsync(IEnumerable<int> userIds)
        {
            return await _sqlContext.Users
                .Where(user => userIds.Contains(user.Id))
                .Include(user => user.Profiles)
                .ToListAsync();
        }
        public async Task<IEnumerable<Tour>> GetTourBriefByUserId(int creatorId)
        {
            return _mongoContext.Find(t => t.ApprovalStatus == ApprovalStatus.Accepted && t.Creator.Id == creatorId).ToList();
        }

        public IEnumerable<Tour> GetAllToursOfLocal(int userId)
        {
            return _mongoContext.Find(t => t.Creator.Id == userId).ToList().OrderByDescending(t => t.CreatedAt);
        }

        public async Task<IEnumerable<Tour>> GetToursByStatus(int userId, ApprovalStatus? approvalStatus)
        {
            return _mongoContext.Find(t => t.Creator.Id == userId && t.ApprovalStatus == approvalStatus).ToList().OrderByDescending(t => t.CreatedAt);
        }

        // Get all tours
        public IEnumerable<Tour> GetAllTours()
        {
            return _mongoContext.Find(_ => true).ToList().OrderByDescending(t => t.CreatedAt);
        }

        // Get a tour by ID
        public async Task<Tour> GetTourById(string id)
        {
            return _mongoContext.Find(t => t.TourId == id).FirstOrDefault();
        }

        // Add a new tour
        public async Task AddTour(Tour tour)
        {
            _mongoContext.InsertOne(tour);
        }

        // Update an existing tour
        public async Task UpdateTour(string id, Tour tour)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, id);
            _mongoContext.ReplaceOne(filter, tour);
        }

        // Delete a tour
        public async Task DeleteTour(string id)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, id);
            _mongoContext.DeleteOne(filter);
        }

        // Admin accepts a tour
        public async Task ProcessTourAdmin(string tourId, ApprovalStatus processStatus)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.ApprovalStatus, processStatus);
            await _mongoContext.UpdateOneAsync(filter, update);
        }

        // Cancel a tour
        public async Task CancelTour(string tourId)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.TourStatus, false);
            await _mongoContext.UpdateOneAsync(filter, update);
        }

        // Get TourName by TourId
        public async Task<string> GetTourNameById(string tourId)
        {
            var tour = _mongoContext.Find(t => t.TourId == tourId).FirstOrDefault();
            return tour?.TourName;
        }

    }
}
