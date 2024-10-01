using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindLocalController : ControllerBase
    {
        private readonly IFindLocalRepository _repository;

        public FindLocalController(IFindLocalRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("traveler/{userId}")]
        public async Task<ActionResult<ApplicationUser>> GetTraveler(string userId)
        {
            var traveler = await _repository.GetTravelerByIdAsync(userId);
            if (traveler == null) return NotFound();
            return Ok(traveler);
        }

        [HttpPost("locals/matching")]
        public async Task<ActionResult<List<ApplicationUser>>> GetLocalsWithMatchingLocations([FromBody] List<int> locationIds)
        {
            var locals = await _repository.GetLocalsWithMatchingLocationsAsync(locationIds);
            return Ok(locals);
        }

        [HttpGet("activities/{userId}")]
        public async Task<ActionResult<List<int>>> GetUserActivities(string userId)
        {
            var activities = await _repository.GetUserActivityIdsAsync(userId);
            return Ok(activities);
        }
    }
}
