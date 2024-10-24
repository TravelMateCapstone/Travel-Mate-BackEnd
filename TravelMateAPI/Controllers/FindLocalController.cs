using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;
using TravelMateAPI.Services.FindLocal;

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

    //[Route("odata/[controller]")]
    //[ApiController]
    //public class FindLocalODataController : ODataController
    //{
    //    private readonly IFindLocalRepository _repository;

    public class FindLocalController : ODataController
        {
        //private readonly IFindLocalService _findLocal;

        //public FindLocalController(IFindLocalService findLocal)
        //{
        //    _findLocal = findLocal;
        //}
        //[HttpGet("search-locals")]
        //[EnableQuery]
        //public async Task<IActionResult> SearchLocals(int travelerId, int locationId, ODataQueryOptions<ApplicationUser> queryOptions)
        //{
        //    // Lấy danh sách người dùng phù hợp từ service
        //    var users = await _findLocal.SearchLocalsWithMatchingActivities(travelerId, locationId);

        //    // Áp dụng các query từ ODataQueryOptions vào danh sách người dùng đã tải
        //    //var filteredUsers = queryOptions.ApplyTo(users.AsQueryable()).Cast<ApplicationUser>().ToList();

        //    return Ok(users);
        //}

        //// Lấy traveler dựa trên userId
        //[HttpGet("traveler/{userId}")]
        //[EnableQuery]
        //public async Task<ActionResult<ApplicationUser>> GetTraveler(int userId)
        //{
        //    var traveler = await _repository.GetTravelerByIdAsync(userId);
        //    if (traveler == null) return NotFound();
        //    return Ok(traveler);
        //}

        //// Lấy danh sách local users phù hợp với sở thích địa điểm
        //[HttpPost("locals/matching")]
        //[EnableQuery]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetLocalsWithMatchingLocations([FromBody] List<int> locationIds)
        //{
        //    var locals = await _repository.GetLocalsWithMatchingLocationsAsync(locationIds);
        //    return Ok(locals);
        //}

        //// Lấy danh sách hoạt động của user dựa trên userId
        //[HttpGet("activities/{userId}")]
        //[EnableQuery]
        //public async Task<ActionResult<IEnumerable<int>>> GetUserActivities(int userId)
        //{
        //    var activities = await _repository.GetUserActivityIdsAsync(userId);
        //    return Ok(activities);
        //}
    }
}
