using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelMateAPI.Services.RecommenTourService;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommenTourController : ControllerBase
    {
        private readonly RecommenTourService _recommendationService;

        public RecommenTourController(RecommenTourService recommendationService)
        {
            _recommendationService = recommendationService;
        }
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        [HttpGet("traveler-tours-activities")]
        public async Task<IActionResult> GetTravelerToursAndActivities()
        {
            var result = await _recommendationService.GetTravelerToursAndActivitiesAsync();
            return Ok(result);
        }
        [HttpGet("recommended-tours/{travelerId}")]
        public async Task<IActionResult> GetRecommendedTours(int travelerId)
        {
            var recommendedTours = await _recommendationService.GetRecommendedToursAsync(travelerId);
            return Ok(recommendedTours);
        }

        [HttpGet("detailed-tours/{travelerId}")]
        public async Task<IActionResult> GetRecommendedTourDetails(int travelerId)
        {
            // Lấy danh sách tour được đề xuất
            var recommendedTourIds = await _recommendationService.GetRecommendedToursAsync(travelerId);

            if (recommendedTourIds == null || !recommendedTourIds.Any())
            {
                return NotFound("No recommendations found.");
            }

            // Lấy chi tiết từng tour
            var detailedTours = await _recommendationService.GetDetailedToursAsync(recommendedTourIds);
            return Ok(detailedTours);
        }

        [HttpGet("current-user/{travelerId}")]
        public async Task<IActionResult> GetRecommendedTourDetailsCurrent()
        {
            // Lấy UserId từ JWT token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized(new { Message = "Invalid token or user not found." });
            }
            // Lấy danh sách tour được đề xuất
            var recommendedTourIds = await _recommendationService.GetRecommendedToursAsync(userId);

            if (recommendedTourIds == null || !recommendedTourIds.Any())
            {
                return NotFound("No recommendations found.");
            }

            // Lấy chi tiết từng tour
            var detailedTours = await _recommendationService.GetDetailedToursAsync(recommendedTourIds);
            return Ok(detailedTours);
        }
    }
}
