using BussinessObjects.Entities;
using BussinessObjects.EnumClass;
using BussinessObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelMateAPI.Services.Notification;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public FriendshipController(ApplicationDBContext context, UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // Gửi lời mời kết bạn
        [HttpPost("send")]
        public async Task<IActionResult> SendFriendRequest(int toUserId)
        {
            var fromUser = await _userManager.GetUserAsync(User);
            if (fromUser == null) return Unauthorized();

            // Kiểm tra xem lời mời đã tồn tại chưa
            var existingFriendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.UserId1 == fromUser.Id && f.UserId2 == toUserId);

            if (existingFriendship != null)
            {
                return BadRequest("Lời mời kết bạn đã tồn tại");
            }

            var friendship = new Friendship
            {
                UserId1 = fromUser.Id,
                UserId2 = toUserId,
                Status = FriendshipStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();

            // Tạo thông báo
            await _notificationService.CreateNotificationAsync(toUserId, $"Bạn đã nhận được một lời mời kết bạn từ {fromUser.UserName}");

            return Ok("Lời mời kết bạn đã được gửi");
        }

        // Chấp nhận lời mời kết bạn
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptFriendRequest(int fromUserId)
        {
            var toUser = await _userManager.GetUserAsync(User);
            if (toUser == null) return Unauthorized();

            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.UserId1 == fromUserId && f.UserId2 == toUser.Id && f.Status == FriendshipStatus.Pending);

            if (friendship == null)
            {
                return NotFound("Lời mời kết bạn không tồn tại hoặc đã được xử lý");
            }

            friendship.Status = FriendshipStatus.Accepted;
            friendship.ConfirmedAt = DateTime.UtcNow;

            _context.Friendships.Update(friendship);
            await _context.SaveChangesAsync();

            // Tạo thông báo
            await _notificationService.CreateNotificationAsync(fromUserId, $"Lời mời kết bạn của bạn với {toUser.UserName} đã được chấp nhận");

            return Ok("Bạn đã chấp nhận lời mời kết bạn");
        }

        // Từ chối lời mời kết bạn
        [HttpPost("reject")]
        public async Task<IActionResult> RejectFriendRequest(int fromUserId)
        {
            var toUser = await _userManager.GetUserAsync(User);
            if (toUser == null) return Unauthorized();

            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => f.UserId1 == fromUserId && f.UserId2 == toUser.Id && f.Status == FriendshipStatus.Pending);

            if (friendship == null)
            {
                return NotFound("Lời mời kết bạn không tồn tại hoặc đã được xử lý");
            }

            friendship.Status = FriendshipStatus.Rejected;

            _context.Friendships.Update(friendship);
            await _context.SaveChangesAsync();

            // Tạo thông báo
            await _notificationService.CreateNotificationAsync(fromUserId, $"Lời mời kết bạn của bạn với {toUser.UserName} đã bị từ chối");

            return Ok("Bạn đã từ chối lời mời kết bạn");
        }

        // Xem danh sách bạn bè
        [HttpGet("friends")]
        public async Task<IActionResult> GetFriends()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var friends = await _context.Friendships
                .Where(f => (f.UserId1 == user.Id || f.UserId2 == user.Id) && f.Status == FriendshipStatus.Accepted)
                .ToListAsync();

            return Ok(friends);
        }
    }
}

