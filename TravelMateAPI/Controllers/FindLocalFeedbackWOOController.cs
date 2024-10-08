using Microsoft.AspNetCore.Mvc;
using TravelMateAPI.Services.FindLocal;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindLocalFeedbackWOOController : Controller
    {
        private readonly FindLocalByFeedbackService _findLocalByFeedbackService;

        // Inject FindLocalByFeedbackService qua constructor
        public FindLocalFeedbackWOOController(FindLocalByFeedbackService findLocalByFeedbackService)
        {
            _findLocalByFeedbackService = findLocalByFeedbackService ?? throw new ArgumentNullException(nameof(findLocalByFeedbackService));
        }

        // API để tìm kiếm người dùng local theo đánh giá
        //[HttpGet("search-locals-by-feedback")]
        //public async Task<IActionResult> SearchLocalsByFeedback([FromQuery] int locationId)
        //{
        //    var matchingUsers = await _findLocalByFeedbackService.GetLocalsByFeedbackAsync(locationId);

        //    if (matchingUsers == null || matchingUsers.Count == 0)
        //    {
        //        return NotFound(new { message = "No matching local users found with feedback." });
        //    }

        //    return Ok(matchingUsers);
        //}
        //có phân trang
        [HttpGet("search-locals-by-feedback")]
        public async Task<IActionResult> SearchLocalsByFeedback([FromQuery] int locationId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var matchingUsers = await _findLocalByFeedbackService.GetLocalsByFeedbackAsync(locationId, pageNumber, pageSize);

            if (matchingUsers == null || matchingUsers.Count == 0)
            {
                return NotFound(new { message = "No local users found." });
            }

            return Ok(matchingUsers);
        }
    }
}
