﻿using BusinessObjects;
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

        public async Task<List<TourTransaction>> GetAllTransactionsAsync()
        {
            return await _mongoContext.Find(_ => true).ToListAsync();
        }

        public async Task<TourTransaction?> GetTransactionByIdAsync(int userId)
        {
            return await _mongoContext.Find(t => t.TravelerId == userId).FirstOrDefaultAsync();
        }

        public async Task AddTransactionAsync(TourTransaction transaction)
        {
            await _mongoContext.InsertOneAsync(transaction);
        }

    }
}
