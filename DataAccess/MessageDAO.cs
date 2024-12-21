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

        public async Task<List<int>> GetChatLists(int userId)
        {
            // Fetch messages matching the user ID as sender or receiver
            var result = await _mongoContext
                .Find(m => m.SenderId == userId || m.ReceiverId == userId)
                .Project(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .SortBy(m => m.SentAt)
                .ToListAsync();
            return result.Distinct().ToList();
        }

    }
}
