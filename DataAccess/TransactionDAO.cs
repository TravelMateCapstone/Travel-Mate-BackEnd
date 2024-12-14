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
            return _mongoContext.Find(_ => true).ToList().OrderByDescending(t => t.TransactionTime);
        }

        public async Task<IEnumerable<TourTransaction?>> GetTransactionByIdAsync(int id)
        {
            return _mongoContext.Find(t => t.TravelerId == id || t.localId == id).ToList().OrderByDescending(t => t.TransactionTime);
        }

        public async Task AddTransactionAsync(TourTransaction transaction)
        {
            await _mongoContext.InsertOneAsync(transaction);
        }

    }
}
