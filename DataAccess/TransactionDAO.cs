using BusinessObjects;
using BusinessObjects.Entities;
using MongoDB.Driver;

namespace DataAccess
{
    public class TransactionDAO
    {
        private readonly IMongoCollection<TourTransaction> _mongoContext;

        public TransactionDAO(MongoDbContext mongoContext)
        {
            _mongoContext = mongoContext.GetCollection<TourTransaction>("Transaction");
        }

        public async Task<IEnumerable<TourTransaction>> GetAllTransactionsAsync()
        {
            return await _mongoContext.Find(_ => true).ToListAsync();
        }

        public async Task<TourTransaction?> GetTransactionByIdAsync(string transactionId)
        {
            return _mongoContext.Find(t => t.Id == transactionId).FirstOrDefault();
        }

        public async Task<TourTransaction?> getTransactionByScheduleId(string scheduleId, int travelerId)
        {
            return await _mongoContext
            .Find(t => t.ScheduleId == scheduleId && t.TravelerId == travelerId)
            .FirstOrDefaultAsync();
        }

        public async Task AddTransactionAsync(TourTransaction transaction)
        {
            await _mongoContext.InsertOneAsync(transaction);
        }

        public async Task UpdateTransaction(string transactionId, TourTransaction transaction)
        {
            var filter = Builders<TourTransaction>.Filter.Eq(f => f.Id, transactionId);
            _mongoContext.ReplaceOne(filter, transaction);
        }
    }
}
