using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;

        public ChatController(IMessageRepository messageRepository, IMapper mapper, ApplicationDBContext context)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("LoadMessages")]
        public async Task<IActionResult> LoadMessages([FromQuery] int senderId, [FromQuery] int receiverId)
        {
            var messages = await _messageRepository.GetConversationAsync(senderId, receiverId);
            return Ok(messages);
        }

        [HttpGet("UserInfo/{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var user = _context.Users
                     .Include(t => t.Profiles)
                     .FirstOrDefault(u => u.Id == userId);

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return Ok(userViewModel);
        }

    }
}
