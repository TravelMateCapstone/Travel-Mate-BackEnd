using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;
using BusinessObjects.Entities;

namespace TravelMateAPI.Controllers
{
    //[ApiController]
    //[Route("odata/[controller]")]
    //public class ActivitiesController : ODataController
    //{
    //    private readonly IActivityRepository _activityRepository;

    //    public ActivitiesController(IActivityRepository activityRepository)
    //    {
    //        _activityRepository = activityRepository;
    //    }

    //    // GET: odata/Activities
    //    [EnableQuery]
    //    public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUser> queryOptions)
    //    {
    //        var activities = await _activityRepository.GetAllActivitiesAsync();
    //        return Ok(activities.AsQueryable());
    //    }
    //    //public IActionResult GetAll()
    //    //{
    //    //    var activities = _activityRepository.GetAllActivitiesAsync().Result.AsQueryable();
    //    //    return Ok(activities);
    //    //}

    //    // GET: odata/Activities(1)
    //    [EnableQuery]
    //    public async Task<IActionResult> Get([FromODataUri] int key)
    //    {
    //        var activity = await _activityRepository.GetActivityByIdAsync(key);
    //        if (activity == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(activity);
    //    }

    //    // POST: odata/Activities
    //    public async Task<IActionResult> Post([FromBody] Activity activity)
    //    {
    //        var createdActivity = await _activityRepository.AddActivityAsync(activity);
    //        return Created(createdActivity);
    //    }

    //    // PUT: odata/Activities(1)
    //    public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Activity activity)
    //    {
    //        if (key != activity.ActivityId)
    //        {
    //            return BadRequest();
    //        }
    //        await _activityRepository.UpdateActivityAsync(activity);
    //        return NoContent();
    //    }

    //    // DELETE: odata/Activities(1)
    //    public async Task<IActionResult> Delete([FromODataUri] int key)
    //    {
    //        await _activityRepository.DeleteActivityAsync(key);
    //        return NoContent();
    //    }
    //}
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        // GET: api/Activity
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _activityRepository.GetAllActivitiesAsync();
            return Ok(activities);
        }
        //public IActionResult GetAll()
        //{
        //    var activities = _activityRepository.GetAllActivitiesAsync().Result.AsQueryable();
        //    return Ok(activities);
        //}

        // GET: api/Activity/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _activityRepository.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound(new { Message = $"Activity with id {id} not found." });
            }
            return Ok(activity);
        }

        // POST: api/Activity
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Activity newActivity)
        {
            if (newActivity == null)
        {
                return BadRequest("Activity is null.");
            }

            var createdActivity = await _activityRepository.AddActivityAsync(newActivity);
            return CreatedAtAction(nameof(GetById), new { id = createdActivity.ActivityId }, createdActivity);
        }

        // PUT: api/Activity/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Activity updatedActivity)
        {
            if (id != updatedActivity.ActivityId)
        {
                return BadRequest("Activity ID mismatch.");
            }

            var activity = await _activityRepository.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound(new { Message = $"Activity with id {id} not found." });
            }

            await _activityRepository.UpdateActivityAsync(updatedActivity);
            return NoContent();
        }

        // DELETE: api/Activity/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _activityRepository.GetActivityByIdAsync(id);
            if (activity == null)
        {
                return NotFound(new { Message = $"Activity with id {id} not found." });
            }

            await _activityRepository.DeleteActivityAsync(id);
            return NoContent();
        }
    }
}
