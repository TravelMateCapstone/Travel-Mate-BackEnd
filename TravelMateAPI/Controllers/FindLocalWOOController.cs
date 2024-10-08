using BussinessObjects;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interface;
using TravelMateAPI.Services.FindLocal;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindLocalWOOController : Controller
    {
        
        private readonly IFindLocalService _findLocalService;

        // Inject FindLocalService qua constructor
        public FindLocalWOOController(IFindLocalService findLocalService)
        {
            _findLocalService = findLocalService ?? throw new ArgumentNullException(nameof(findLocalService));
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
        [HttpGet("search-locals")]
        public async Task<IActionResult> SearchLocals([FromQuery] int travelerId, [FromQuery] int locationId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Giả sử bạn đã có một phương thức để lấy danh sách activityIds của traveler
            List<int> travelerActivities = await GetTravelerActivitiesAsync(travelerId);

            // Gọi FindLocalService để tìm người dùng local theo role "Local"
            var matchingUsers = await _findLocalService.GetMatchingUsersAsync(locationId, travelerActivities, "Local", pageNumber, pageSize);

            if (matchingUsers == null || matchingUsers.Count == 0)
            {
                return NotFound(new { message = "No matching local users found." });
            }

            return Ok(matchingUsers);
        }


        // Giả sử bạn có phương thức lấy danh sách activityIds cho traveler
        private Task<List<int>> GetTravelerActivitiesAsync(int travelerId)
        {
            // Logic để lấy danh sách activityIds từ cơ sở dữ liệu
            // Trả về danh sách hoạt động của traveler
            return Task.FromResult(new List<int> { 1, 2, 3 });
        }
    }
}
