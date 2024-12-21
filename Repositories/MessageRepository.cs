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

        public async Task AddMessageAsync(Message message)
        {
            await _messageDAO.AddMessageAsync(message);
        }

        public async Task<List<Message>> GetConversationAsync(int userId1, int userId2)
        {
            return await _messageDAO.GetConversationAsync(userId1, userId2);
        }
    }
}
