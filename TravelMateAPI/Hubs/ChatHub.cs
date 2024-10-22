using BussinessObjects;
using BussinessObjects.Entities;
using Microsoft.AspNetCore.SignalR;

namespace TravelMateAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDBContext _context;
        public ChatHub(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string userId, string message)
        {
            var senderId = Context.UserIdentifier;

            var chatMessage = new Message
            {
                SenderId = int.Parse(senderId),
                ReceiverId = int.Parse(userId),
                Text = message,
                CreatedTime = DateTime.UtcNow
            };

            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }

        //
    }
}
