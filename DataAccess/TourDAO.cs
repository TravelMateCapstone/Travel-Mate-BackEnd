using BusinessObjects;
using BusinessObjects.Entities;
using MongoDB.Driver;

namespace DataAccess
{
    public class TourDAO
    {
        private readonly ApplicationDBContext _sqlContext;
        private readonly MongoDbContext _mongoContext;

        public TourDAO(ApplicationDBContext context, MongoDbContext mongoContext)
        {
            _sqlContext = context;
            _mongoContext = mongoContext;
        }

        //get list participants of a tour


        //get list tour of a participants

        //get list tour of local based on approval status
        public IEnumerable<Tour> GetAllToursOfLocal(int userId)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            return collection.Find(t => t.Creator.Id == userId).ToList();
        }

        public async Task<IEnumerable<Tour>> GetToursByStatus(int userId, bool? approvalStatus)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            return collection.Find(t => t.Creator.Id == userId && t.ApprovalStatus == approvalStatus).ToList();
        }



        // Get all tours
        public IEnumerable<Tour> GetAllTours()
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            return collection.Find(_ => true).ToList();
        }

        // Get a tour by ID
        public async Task<Tour> GetTourById(string id)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            return collection.Find(t => t.TourId == id).FirstOrDefault();
        }

        // Add a new tour
        public async Task AddTour(Tour tour)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            collection.InsertOne(tour);
        }

        // Update an existing tour
        public async Task UpdateTour(string id, Tour tour)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, id);
            collection.ReplaceOne(filter, tour);
        }

        // Delete a tour
        public async Task DeleteTour(string id)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, id);
            collection.DeleteOne(filter);
        }

        public async Task JoinTour(string tourId, Participants participant)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");

            // Tạo bộ lọc để tìm tour theo tourId
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            // Tạo update để thêm travelerId vào mảng participants
            var update = Builders<Tour>.Update.Push(f => f.Participants, participant);

            // Thực hiện cập nhật tour với toán tử $push để thêm travelerId vào participants
            await collection.UpdateOneAsync(filter, update);
        }

        // Admin accepts a tour
        public async Task AcceptTour(string tourId)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.ApprovalStatus, true);
            await collection.UpdateOneAsync(filter, update);
        }

        // Add a review to a tour
        public async Task AddReview(string tourId, TourReview tourReview)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.AddToSet(t => t.Reviews, tourReview);
            await collection.UpdateOneAsync(filter, update);
        }

        // Update availability of a tour
        public async Task UpdateAvailability(string tourId, int slots)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.RegisteredGuests, slots);
            await collection.UpdateOneAsync(filter, update);
        }

        // Cancel a tour
        public async Task CancelTour(string tourId)
        {
            var collection = _mongoContext.GetCollection<Tour>("Tours");
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, tourId);

            var update = Builders<Tour>.Update.Set(t => t.TourStatus, false);
            await collection.UpdateOneAsync(filter, update);
        }
    }
}
