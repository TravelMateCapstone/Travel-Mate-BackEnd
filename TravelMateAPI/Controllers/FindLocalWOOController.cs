using BussinessObjects;
using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interface;
using TravelMateAPI.Services.FindLocal;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindLocalWOOController : ControllerBase
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


        // Giả sử bạn có phương thức lấy danh sách activityIds cho traveler
        private Task<List<int>> GetTravelerActivitiesAsync(int travelerId)
        {
            // Logic để lấy danh sách activityIds từ cơ sở dữ liệu
            // Trả về danh sách hoạt động của traveler
            return Task.FromResult(new List<int> { 1, 2, 3 });
        }
    }
}
