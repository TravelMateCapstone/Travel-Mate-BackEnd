using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects;
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
            await _notificationService.CreateNotificationAsync(toUserId, $"Bạn đã nhận được một lời mời kết bạn từ {fromUser.FullName}");

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
            await _notificationService.CreateNotificationAsync(fromUserId, $"Lời mời kết bạn của bạn với {toUser.FullName} đã được chấp nhận");

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
            await _notificationService.CreateNotificationAsync(fromUserId, $"Lời mời kết bạn của bạn với {toUser.FullName} đã bị từ chối");

            return Ok("Bạn đã từ chối lời mời kết bạn");
        }

        // Xóa bạn bè
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFriend(int friendUserId)
        {
            // Lấy thông tin người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            // Tìm mối quan hệ bạn bè giữa người dùng hiện tại và bạn cần xóa
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => (f.UserId1 == currentUser.Id && f.UserId2 == friendUserId) ||
                                          (f.UserId1 == friendUserId && f.UserId2 == currentUser.Id) &&
                                          f.Status == FriendshipStatus.Accepted);

            if (friendship == null)
            {
                return NotFound("Không tìm thấy bạn bè.");
            }

            // Xóa mối quan hệ bạn bè
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            // Gửi thông báo đến người dùng đã bị xóa (nếu cần)
            await _notificationService.CreateNotificationAsync(friendUserId, $"Bạn đã bị {currentUser.FullName} xóa khỏi danh sách bạn bè.");

            return Ok("Bạn đã xóa bạn bè thành công.");
        }
        // Lấy danh sách bạn bè của người dùng hiện tại
        [HttpGet("current-user/friends")]
        public async Task<IActionResult> GetCurrentUserFriends()
        {
            // Lấy thông tin người dùng hiện tại từ JWT token
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized("Không tìm thấy người dùng hiện tại.");

            // Lấy danh sách bạn bè với trạng thái 'Accepted'
            var friends = await _context.Friendships
                .Where(f => (f.UserId1 == currentUser.Id || f.UserId2 == currentUser.Id) && f.Status == FriendshipStatus.Accepted)
                .Select(f => new
                {
                    FriendId = f.UserId1 == currentUser.Id ? f.UserId2 : f.UserId1,
                    FriendName = f.UserId1 == currentUser.Id ? f.User2.UserName : f.User1.UserName,
                    FriendshipId = f.FriendshipId,
                    ConfirmedAt = f.ConfirmedAt,

                })
                .ToListAsync();
            // Lấy profile của từng bạn bè dựa vào FriendId
            var friendsWithProfiles = new List<object>();
            foreach (var friend in friends)
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == friend.FriendId);
                friendsWithProfiles.Add(new
                {
                    FriendId = friend.FriendId,
                    FriendName = friend.FriendName,
                    FriendshipId = friend.FriendshipId,
                    ConfirmedAt = friend.ConfirmedAt,
                    Profile = profile
                });
            }
            // Nếu không có bạn bè nào
            if (friends == null || !friends.Any())
            {
                return NotFound("Bạn không có bạn bè nào.");
            }

            //return Ok(friends);
            return Ok(friendsWithProfiles);
        }
        [HttpGet("current-user/friends-with-profiles")]
        public async Task<IActionResult> GetFriendsWithProfilesForCurrentUser()
        {
            // Lấy thông tin người dùng hiện tại từ JWT token
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized("Không tìm thấy người dùng hiện tại.");

            // Lấy danh sách bạn bè và thông tin Profile của họ dựa trên friendId
            var friendsWithProfiles = await _context.Friendships
                .Where(f => (f.UserId1 == currentUser.Id || f.UserId2 == currentUser.Id) && f.Status == FriendshipStatus.Accepted)
                .Select(f => new
                {
                    FriendId = f.UserId1 == currentUser.Id ? f.UserId2 : f.UserId1,
                    FriendshipId = f.FriendshipId,
                    ConfirmedAt = f.ConfirmedAt
                })
                .ToListAsync();

            // Lấy thông tin Profile dựa trên friendId
            var friendsWithProfileDetails = await Task.WhenAll(friendsWithProfiles.Select(async friend => new
            {
                FriendId = friend.FriendId,
                FriendshipId = friend.FriendshipId,
                ConfirmedAt = friend.ConfirmedAt,
                Profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == friend.FriendId) // Lấy Profile từ friendId
            }));

            return Ok(friendsWithProfileDetails);
        }



        [HttpGet("List-friends/{userId}")]
        public async Task<IActionResult> GetFriendsByUserId(int userId)
        {
            // Kiểm tra xem user có tồn tại không
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại");
            }

            // Lấy danh sách bạn bè
            var friends = await _context.Friendships
                .Where(f => (f.UserId1 == userId || f.UserId2 == userId) && f.Status == FriendshipStatus.Accepted)
                .Select(f => new
                {
                    FriendId = f.UserId1 == userId ? f.UserId2 : f.UserId1,
                    FriendName = f.UserId1 == userId ? f.User2.UserName : f.User1.UserName,
                    FriendshipId = f.FriendshipId,
                    ConfirmedAt = f.ConfirmedAt
                })
                .ToListAsync();
            // Lấy profile của từng bạn bè dựa vào FriendId
            var friendsWithProfiles = new List<object>();
            foreach (var friend in friends)
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == friend.FriendId);
                friendsWithProfiles.Add(new
                {
                    FriendId = friend.FriendId,
                    FriendName = friend.FriendName,
                    FriendshipId = friend.FriendshipId,
                    ConfirmedAt = friend.ConfirmedAt,
                    Profile = profile
                });
            }
            return Ok(friendsWithProfiles);
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

        [HttpGet("check-friendship")]
        public async Task<IActionResult> CheckFriendshipStatus( int otherUserId)
        {
            // Lấy thông tin người dùng hiện tại từ JWT token
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized("Không tìm thấy người dùng hiện tại.");

            int userId = currentUser.Id;
            // Kiểm tra xem userId có tồn tại không
            //var user = await _userManager.FindByIdAsync(userId.ToString());
            //if (user == null) return NotFound("Người dùng không tồn tại.");

            // Kiểm tra xem otherUserId có tồn tại không
            var otherUser = await _userManager.FindByIdAsync(otherUserId.ToString());
            if (otherUser == null) return NotFound("Người dùng kia không tồn tại.");

            // Kiểm tra xem đã kết bạn chưa
            var isFriend = await _context.Friendships.AnyAsync(f =>
                ((f.UserId1 == userId && f.UserId2 == otherUserId) ||
                 (f.UserId1 == otherUserId && f.UserId2 == userId)) &&
                f.Status == FriendshipStatus.Accepted);

            return Ok(new { AreFriends = isFriend });
        }
    }
}

