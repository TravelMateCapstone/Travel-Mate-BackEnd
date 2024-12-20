using BusinessObjects;
using BusinessObjects.Entities;
using MongoDB.Driver;

namespace DataAccess
{
    public class MessageDAO
    {
        private readonly IMongoCollection<Message> _mongoContext;
        public MessageDAO(MongoDbContext mongoContext)
        {
            _mongoContext = mongoContext.GetCollection<Message>("Messages");
        }

        public async Task AddMessageAsync(Message message)
        {
            await _mongoContext.InsertOneAsync(message);
        }

        public async Task<List<Message>> GetConversationAsync(int userId1, int userId2)
        {
            return await _mongoContext.Find(m =>
            (m.SenderId == userId1 && m.ReceiverId == userId2) ||
            (m.SenderId == userId2 && m.ReceiverId == userId1))
            .SortBy(m => m.SentAt)
            .ToListAsync();
        }
    }
}
