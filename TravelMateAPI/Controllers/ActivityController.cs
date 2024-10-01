using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;
using BussinessObjects.Entities;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("odata/[controller]")]
    public class ActivityController : ODataController
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        // GET: odata/Activities
        [EnableQuery]
        public IActionResult GetAll()
        {
            var activities = _activityRepository.GetAllActivitiesAsync().Result.AsQueryable();
            return Ok(activities);
        }

        // GET: odata/Activities(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var activity = await _activityRepository.GetActivityByIdAsync(key);
            if (activity == null)
            {
                return NotFound();
            }
            return Ok(activity);
        }

        // POST: odata/Activities
        public async Task<IActionResult> Post([FromBody] Activity activity)
        {
            var createdActivity = await _activityRepository.AddActivityAsync(activity);
            return Created(createdActivity);
        }

        // PUT: odata/Activities(1)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Activity activity)
        {
            if (key != activity.ActivityId)
            {
                return BadRequest();
            }
            await _activityRepository.UpdateActivityAsync(activity);
            return NoContent();
        }

        // DELETE: odata/Activities(1)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            await _activityRepository.DeleteActivityAsync(key);
            return NoContent();
        }
    }
}
