using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(int senderId, int receiverId, string content);
        Task<List<Message>> GetConversationAsync(int userId1, int userId2);
    }
}
