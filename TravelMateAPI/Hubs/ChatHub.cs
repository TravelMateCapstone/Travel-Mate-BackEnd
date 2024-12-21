using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace TravelMateAPI.Hubs
{
    public sealed class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();
        private readonly static Dictionary<int, string> _ConnectionsMap = new Dictionary<int, string>();
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;

        public ChatHub(IMessageRepository messageRepository, IMapper mapper, ApplicationDBContext context)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task SendPrivate(int receiverId, string message)
        {
            var senderId = UserId;

            if (!string.IsNullOrEmpty(message.Trim()))
            {
                var messageEntity = new Message
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = message,
                    SentAt = DateTime.Now
                };

                await _messageRepository.AddMessageAsync(messageEntity);

                if (_ConnectionsMap.TryGetValue(receiverId, out string connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("receiveMessage", messageEntity);
                }

                await Clients.Caller.SendAsync("receiveMessage", "Sent");
            }
        }

        public async Task GetChatUsers()
        {
            var currentUserId = UserId;
            var chatUsers = await _messageRepository.GetChatLists(currentUserId);

            await Clients.Caller.SendAsync("receiveChatUsers", chatUsers);
        }

        private int UserId
        {
            get { return int.Parse(Context.User?.FindFirst("UserId")?.Value); }
            //get { return 92; }
        }

        public async Task LoadMessages(int senderId, int receiverId)
        {
            var messages = await _messageRepository.GetConversationAsync(senderId, receiverId);
            await Clients.Caller.SendAsync("LoadMessages", messages);
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = _context.Users
                    .Include(t => t.Profiles)
                    .FirstOrDefault(u => u.Id == UserId);

                var userViewModel = _mapper.Map<UserViewModel>(user);

                if (!_Connections.Any(u => u.Id == UserId))
                {
                    _Connections.Add(userViewModel);
                    _ConnectionsMap.Add(UserId, Context.ConnectionId);
                }

                Clients.Caller.SendAsync("getProfileInfo", userViewModel);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _Connections.FirstOrDefault(u => u.Id == UserId);
                if (user != null)
                {
                    _Connections.Remove(user);
                    _ConnectionsMap.Remove(UserId);
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

    }
}
