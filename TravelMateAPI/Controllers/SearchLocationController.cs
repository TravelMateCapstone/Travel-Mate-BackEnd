using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelMateAPI.Services.FilterLocal;
using TravelMateAPI.Services.FindLocal;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchLocationController : ControllerBase
    {
        private readonly ISearchLocationService _slocationService;
        private readonly SearchLocationFuzzyService _searchLocationFuzzyService;
        private readonly LocationService _locationService;
      
        public SearchLocationController(ISearchLocationService slocationService, SearchLocationFuzzyService searchLocationFuzzyService,LocationService locationService)
        {
            _slocationService = slocationService;
            _searchLocationFuzzyService = searchLocationFuzzyService;
            _locationService = locationService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<LocationDTO>>> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var results = await _slocationService.SearchLocationsAsync(query);

            if (results == null || results.Count == 0)
            {
                return NotFound("No locations found.");
            }

            return Ok(results);
        }
        [HttpGet("searchfuzzy")]
        public async Task<ActionResult<List<LocationDTO>>> SearchFuzzy(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var results = await _searchLocationFuzzyService.SearchLocationsAsync(query);

            if (results == null || results.Count == 0)
            {
                return NotFound("No locations found.");
            }

            return Ok(results);
        }

        [HttpGet("search-location")]
        public async Task<IActionResult> SearchLocation([FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest("Keyword is required.");

            try
            {
                var result = await _locationService.SearchLocationAsync(keyword);
                if (result.LocationId == null)
                    return NotFound("Location not found.");

                return Ok(new { LocationId = result.LocationId, LocationName = result.LocationName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
// ví dụ : GET /api/location/search?query=ha noi
// API sẽ trả về danh sách các kết quả phù hợp với chuỗi tìm kiếm gần đúng mà bạn đã nhập, bao gồm cả khi người dùng gõ sai chính tả.