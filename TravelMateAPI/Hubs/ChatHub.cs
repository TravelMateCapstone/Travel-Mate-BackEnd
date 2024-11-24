using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using TravelMateAPI.Services;

namespace TravelMateAPI.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();
        private readonly MongoDbContext _mongoContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(UserManager<ApplicationUser> userManager, MongoDbContext context)
        {
            _mongoContext = context;
            _userManager = userManager;
        }
        public async Task SendPrivateMessage(string senderUserId, string receiverUserId, string message)
        {
            // Validate the sender's user ID (security check)
            var authenticatedUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (authenticatedUserId == null || authenticatedUserId != senderUserId)
            {
                await Clients.Caller.SendAsync("Error", "User ID validation failed.");
                return;
            }

            // Add the message to the database, regardless of online status
            var collection = _mongoContext.GetCollection<Message>("Messages");

            var newMessage = new Message()
            {
                SenderId = int.Parse(authenticatedUserId),
                ReceiverId = int.Parse(receiverUserId),
                Text = message,
                SentAt = GetTimeZone.GetVNTimeZoneNow()
            };

            await collection.InsertOneAsync(newMessage);

            // Check if the recipient is online
            if (_userConnections.TryGetValue(receiverUserId, out string connectionId))
            {
                // Send the message to the online user
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", authenticatedUserId, message);

                // Update the message to mark it as delivered
                //var filter = Builders<Message>.Filter.Eq(m => m.Id, newMessage.Id);
                //var update = Builders<Message>.Update.Set(m => m.IsDelivered, true);
                //await collection.UpdateOneAsync(filter, update);
            }
        }

    }
}
