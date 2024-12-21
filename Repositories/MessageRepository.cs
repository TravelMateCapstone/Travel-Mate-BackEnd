using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDAO _messageDAO;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;

        public MessageRepository(MessageDAO messageDAO, IMapper mapper, ApplicationDBContext context)
        {
            _messageDAO = messageDAO;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            await _messageDAO.AddMessageAsync(message);
        }

        public async Task<List<UserViewModel>> GetChatLists(int userId)
        {
            var listIds = await _messageDAO.GetChatLists(userId);
            var listUsers = new List<UserViewModel>();

            foreach (var item in listIds)
            {
                var user = _context.Users
                    .Include(t => t.Profiles)
                    .FirstOrDefault(u => u.Id == item);

                var userMap = _mapper.Map<UserViewModel>(user);
                listUsers.Add(userMap);
            }

            return listUsers;
        }

        public async Task<List<Message>> GetConversationAsync(int userId1, int userId2)
        {
            return await _messageDAO.GetConversationAsync(userId1, userId2);
        }
    }
}
