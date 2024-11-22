using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interface;
using System.Security.Claims;
using TravelMateAPI.Services.FindLocal;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindLocalWOOController : Controller
    {
        
        private readonly IFindLocalService _findLocalService;
        private readonly ApplicationDBContext _context;

        // Inject FindLocalService qua constructor
        public FindLocalWOOController(IFindLocalService findLocalService, ApplicationDBContext context)
        {
            _findLocalService = findLocalService ?? throw new ArgumentNullException(nameof(findLocalService));
            _context = context;
        }
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        // API để tìm kiếm người dùng local
        //[HttpGet("search-locals")]
        //public async Task<IActionResult> SearchLocals([FromQuery] int travelerId, [FromQuery] int locationId)
        //{
        //    // Giả sử bạn đã có một phương thức để lấy danh sách activityIds của traveler
        //    List<int> travelerActivities = await GetTravelerActivitiesAsync(travelerId);

        //    // Gọi FindLocalService để tìm người dùng local theo role "Local"
        //    var matchingUsers = await _findLocalService.GetMatchingUsersAsync(locationId, travelerActivities, "Local");

        //    if (matchingUsers == null || matchingUsers.Count == 0)
        //    {
        //        return NotFound(new { message = "No matching local users found." });
        //    }

        //    return Ok(matchingUsers);
        //}

        //Tìm có phân trang 
        //[HttpGet("search-locals")]
        //public async Task<IActionResult> SearchLocals([FromQuery] int travelerId, [FromQuery] int locationId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    // Giả sử bạn đã có một phương thức để lấy danh sách activityIds của traveler
        //    List<int> travelerActivities = await GetTravelerActivitiesAsync(travelerId);

        //    // Gọi FindLocalService để tìm người dùng local theo role "Local"
        //    var matchingUsers = await _findLocalService.GetMatchingUsersAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);

        //    if (matchingUsers == null || matchingUsers.Count == 0)
        //    {
        //        return NotFound(new { message = "No matching local users found." });
        //    }

        //    return Ok(matchingUsers);
        //}

        [HttpGet("search-locals")]
        public async Task<IActionResult> SearchLocals([FromQuery] int travelerId, [FromQuery] int locationId, [FromQuery] int option = 0, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Giả sử bạn đã có một phương thức để lấy danh sách activityIds của traveler
            List<int> travelerActivities = await GetTravelerActivitiesAsync(travelerId);

            // Tạo một biến để lưu kết quả tìm kiếm
            List<ApplicationUser> matchingUsers = null;

            // Sử dụng switch-case để thực hiện logic dựa trên giá trị của option
            switch (option)
            {
                case 1:
                    // Thực hiện logic khi option = 1
                    matchingUsers = await _findLocalService.GetMatchingUsersAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);
                    break;

                case 2:
                    // Thực hiện logic khi option = 2
                    matchingUsers = await _findLocalService.GetMatchingUsersAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);
                    break;

                default:
                    // Thực hiện logic mặc định (khi option không phải 1 hoặc 2)
                    matchingUsers = await _findLocalService.GetMatchingUsersAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);
                    break;
            }

            if (matchingUsers == null || matchingUsers.Count == 0)
            {
                return NotFound(new { message = "No matching local users found." });
            }

            return Ok(matchingUsers);
        }

        // API để tìm kiếm người dùng local theo UserId hiện tại
        [HttpGet("search-locals/current-user")]
        public async Task<IActionResult> SearchLocalsForCurrentUser([FromQuery] int locationId, [FromQuery] int option = 0, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Lấy UserId từ token hoặc context hiện tại
            var userId = GetUserId(); // Giả sử có phương thức này để lấy UserId hiện tại
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Giả sử bạn đã có một phương thức để lấy danh sách activityIds của traveler
            List<int> travelerActivities = await GetTravelerActivitiesAsync(userId);

            // Tạo một biến để lưu kết quả tìm kiếm
            List<ApplicationUser> matchingUsers = null;

            // Sử dụng switch-case để thực hiện logic dựa trên giá trị của option
            switch (option)
            {
                case 1:
                    // Thực hiện logic khi option = 1
                    matchingUsers = await _findLocalService.GetMatchingUsersAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);
                    break;

                case 2:
                    // Thực hiện logic khi option = 2
                    matchingUsers = await _findLocalService.GetMatchingUsersAndWoMcAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);
                    break;

                default:
                    // Thực hiện logic mặc định (khi option không phải 1 hoặc 2)
                    matchingUsers = await _findLocalService.GetMatchingUsersAndWoMcAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);
                    break;
            }

            if (matchingUsers == null || matchingUsers.Count == 0)
            {
                return NotFound(new { message = "No matching local users found." });
            }

            return Ok(matchingUsers);
        }
        // Giả sử bạn có phương thức lấy danh sách activityIds cho traveler
        private async Task<List<int>> GetTravelerActivitiesAsync(int travelerId)
        {
            // Lấy danh sách ActivityId từ cơ sở dữ liệu theo UserId (travelerId)
            var activityIds = await _context.UserActivities
                .Where(ua => ua.UserId == travelerId)
                .Select(ua => ua.ActivityId)
                .ToListAsync();

            // Trả về danh sách activityIds của traveler
            return activityIds;
        }

    }
}
