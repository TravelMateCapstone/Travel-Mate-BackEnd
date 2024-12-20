using Microsoft.AspNetCore.SignalR;
using Repositories.Interface;

namespace TravelMateAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        public ChatHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task SendMessage(string user, string content)
        {
            // Gửi thông báo thời gian thực đến người nhận
            await Clients.All.SendAsync("ReceiveMessage", user, content);
        }
    }
}
