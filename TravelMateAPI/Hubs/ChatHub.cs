using BusinessObjects.Entities;
using Microsoft.AspNetCore.SignalR;
using Repositories.Interface;

namespace TravelMateAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        public int senderId = 8;
        public ChatHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task SendMessage(int receiverId, string content)
        {
            await _messageRepository.AddMessageAsync(senderId, receiverId, content);

            // Gửi thông báo thời gian thực đến người nhận
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, content);
        }

        public async Task<List<Message>> GetMessages(string userId)
        {
            return await _messageRepository.GetConversationAsync(currentUserId, userId);
        }

    }
}
