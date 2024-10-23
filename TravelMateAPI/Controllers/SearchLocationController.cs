﻿using Microsoft.AspNetCore.Mvc;
using TravelMateAPI.Services.FindLocal;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchLocationController : ControllerBase
    {
        private readonly ISearchLocationService _locationService;
        private readonly SearchLocationFuzzyService _searchLocationFuzzyService;

        public SearchLocationController(ISearchLocationService locationService, SearchLocationFuzzyService searchLocationFuzzyService)
        {
            _locationService = locationService;
            _searchLocationFuzzyService = searchLocationFuzzyService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<LocationDTO>>> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            var results = await _locationService.SearchLocationsAsync(query);

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
    }
}


// ví dụ : GET /api/location/search?query=ha noi
// API sẽ trả về danh sách các kết quả phù hợp với chuỗi tìm kiếm gần đúng mà bạn đã nhập, bao gồm cả khi người dùng gõ sai chính tả.