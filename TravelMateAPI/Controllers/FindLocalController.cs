﻿using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class FindLocalController : ControllerBase
    //{
    //    private readonly IFindLocalRepository _repository;

    //    public FindLocalController(IFindLocalRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    [HttpGet("traveler/{userId}")]
    //    public async Task<ActionResult<ApplicationUser>> GetTraveler(string userId)
    //    {
    //        var traveler = await _repository.GetTravelerByIdAsync(userId);
    //        if (traveler == null) return NotFound();
    //        return Ok(traveler);
    //    }

    //    [HttpPost("locals/matching")]
    //    public async Task<ActionResult<List<ApplicationUser>>> GetLocalsWithMatchingLocations([FromBody] List<int> locationIds)
    //    {
    //        var locals = await _repository.GetLocalsWithMatchingLocationsAsync(locationIds);
    //        return Ok(locals);
    //    }

    //    [HttpGet("activities/{userId}")]
    //    public async Task<ActionResult<List<int>>> GetUserActivities(string userId)
    //    {
    //        var activities = await _repository.GetUserActivityIdsAsync(userId);
    //        return Ok(activities);
    //    }
    //}

    [Route("odata/[controller]")]
    [ApiController]
    public class FindLocalODataController : ODataController
    {
        private readonly IFindLocalRepository _repository;

        public FindLocalODataController(IFindLocalRepository repository)
        {
            _repository = repository;
        }

        // Lấy traveler dựa trên userId
        [HttpGet("traveler/{userId}")]
        [EnableQuery]
        public async Task<ActionResult<ApplicationUser>> GetTraveler(string userId)
        {
            var traveler = await _repository.GetTravelerByIdAsync(userId);
            if (traveler == null) return NotFound();
            return Ok(traveler);
        }

        // Lấy danh sách local users phù hợp với sở thích địa điểm
        [HttpPost("locals/matching")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetLocalsWithMatchingLocations([FromBody] List<int> locationIds)
        {
            var locals = await _repository.GetLocalsWithMatchingLocationsAsync(locationIds);
            return Ok(locals);
        }

        // Lấy danh sách hoạt động của user dựa trên userId
        [HttpGet("activities/{userId}")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<int>>> GetUserActivities(string userId)
        {
            var activities = await _repository.GetUserActivityIdsAsync(userId);
            return Ok(activities);
        }
    }
}
