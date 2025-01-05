using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DataAccess
{
    public class TourParticipantDAO
    {
        private readonly ApplicationDBContext _sqlContext;
        private readonly IMongoCollection<Tour> _mongoContext;
        private readonly IMongoCollection<PastTripPost> _postMongoContext;

        public TourParticipantDAO(ApplicationDBContext context, MongoDbContext mongoContext)
        {
            _sqlContext = context;
            _mongoContext = mongoContext.GetCollection<Tour>("Tours");
            _postMongoContext = mongoContext.GetCollection<PastTripPost>("PastTripPosts");
        }

        public async Task<Tour> GetTourByParticipantOrderCodeAsync(long orderCode)
        {
            var filter = Builders<Tour>.Filter.ElemMatch(
                t => t.Schedules,
                schedule => schedule.Participants.Any(participant => participant.OrderCode == orderCode)
            );

            return await _mongoContext.Find(filter).FirstOrDefaultAsync();
        }


        public async Task<ApplicationUser> GetUserInfor(int userId)
        {
            return _sqlContext.Users
                .Include(t => t.Profiles)
                .FirstOrDefault(t => t.Id == userId);
        }

        public async Task<Tour> GetTourScheduleById(string scheduleId, string tourId)
        {
            var filter = Builders<Tour>.Filter.And(
                Builders<Tour>.Filter.Eq(t => t.TourId, tourId),
                Builders<Tour>.Filter.ElemMatch(t => t.Schedules, s => s.ScheduleId == scheduleId)
            );

            return await _mongoContext
                .Find(filter)
                .FirstOrDefaultAsync();
        }
        public async Task UpdateTour(string id, Tour tour)
        {
            var filter = Builders<Tour>.Filter.Eq(f => f.TourId, id);
            _mongoContext.ReplaceOne(filter, tour);
        }

        public async Task JoinTour(string scheduleId, string tourId, Participants participant)
        {

            var filter = Builders<Tour>.Filter.And(
                 Builders<Tour>.Filter.Eq(f => f.TourId, tourId),
                 Builders<Tour>.Filter.ElemMatch(f => f.Schedules, s => s.ScheduleId == scheduleId)
             );
            var update = Builders<Tour>.Update.Push("schedules.$.participants", participant);

            await _mongoContext.UpdateOneAsync(filter, update);
        }

    }
}
