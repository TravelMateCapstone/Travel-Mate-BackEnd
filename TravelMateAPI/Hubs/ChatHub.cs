using BusinessObjects;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace TravelMateAPI.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();
        private readonly MongoDbContext _mongoContext;

        public ChatHub(MongoDbContext context)
        {
            _mongoContext = context;
        }

        public async Task SendPrivateMessage(string receiverUserId, string message)
        {
            if (_userConnections.TryGetValue(receiverUserId, out string connectionId))
            {
                // Send the message to the specified user
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", Context.ConnectionId, message);
            }
        }
        //
    }
}
