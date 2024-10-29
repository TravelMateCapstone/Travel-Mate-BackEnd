using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lấy danh sách thông báo của người dùng
        [HttpGet("get-notification-current-user")]
        public async Task<IActionResult> GetNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var notifications = await _context.Notifications
                .Where(n => n.UserId == user.Id)
                .ToListAsync();

            return Ok(notifications);
        }
        // Lấy danh sách thông báo của người dùng hiện tại
        [HttpGet("current-user/notifications")]
        public async Task<IActionResult> GetCurrentUserNotifications()
        {
            // Lấy thông tin người dùng hiện tại từ JWT token
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized("Không tìm thấy người dùng hiện tại.");

            // Lấy danh sách thông báo và sắp xếp theo thời gian giảm dần (mới nhất lên đầu)
            var notifications = await _context.Notifications
                .Where(n => n.UserId == currentUser.Id)
                .OrderByDescending(n => n.CreatedAt) // Sắp xếp thông báo mới nhất lên đầu
                .Select(n => new
                {
                    n.NotificationId,
                    n.Message,
                    n.CreatedAt,
                    n.IsRead
                })
                .ToListAsync();

            // Kiểm tra nếu không có thông báo nào
            if (notifications == null || !notifications.Any())
            {
                return NotFound("Không có thông báo nào.");
            }

            return Ok(notifications);
        }

        // Lấy danh sách thông báo của người dùng theo UserId // cái trên cũng được// cái dưới này để test nếu chưa đăng nhập
        [HttpGet("getByUser/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();

            if (notifications == null || notifications.Count == 0)
            {
                return NotFound("Không có thông báo nào cho người dùng này.");
            }

            return Ok(notifications);
        }

        // Đánh dấu thông báo là đã đọc
        [HttpPost("current-user-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return NotFound();

            notification.IsRead = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();

            return Ok("Thông báo đã được đánh dấu là đã đọc");
        }
    }

}
