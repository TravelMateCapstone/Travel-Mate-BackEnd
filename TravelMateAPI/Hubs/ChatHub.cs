using BusinessObjects;
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

        //
    }
}
