using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDAO _messageDAO;

        public MessageRepository(MessageDAO messageDAO)
        {
            _messageDAO = messageDAO;
        }

        public async Task AddMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _messageDAO.AddMessageAsync(message);
        }

        public async Task<List<Message>> GetConversationAsync(int userId1, int userId2)
        {
            return await _messageDAO.GetConversationAsync(userId1, userId2);
        }
    }
}
