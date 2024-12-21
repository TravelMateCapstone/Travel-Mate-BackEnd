using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<List<Message>> GetConversationAsync(int userId1, int userId2);
    }
}
