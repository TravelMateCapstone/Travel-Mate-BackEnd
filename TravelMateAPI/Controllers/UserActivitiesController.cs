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
    public class UserActivitiesController : ODataController
    {
        private readonly IUserActivitiesRepository _userActivitiesRepository;

        public UserActivitiesController(IUserActivitiesRepository userActivitiesRepository)
        {
            _userActivitiesRepository = userActivitiesRepository;
        }

        // GET: odata/UserActivities
        [EnableQuery]
        public IActionResult GetAll()
        {
            var userActivities = _userActivitiesRepository.GetAllUserActivitiesAsync().Result.AsQueryable();
            return Ok(userActivities);
        }

        // GET: odata/UserActivities(UserId=1,ActivityId=1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] string keyUserId, [FromODataUri] int keyActivityId)
        {
            var userActivity = await _userActivitiesRepository.GetUserActivityByIdAsync(keyUserId, keyActivityId);
            if (userActivity == null)
            {
                return NotFound();
            }
            return Ok(userActivity);
        }

        // POST: odata/UserActivities
        public async Task<IActionResult> Post([FromBody] UserActivity userActivity)
        {
            var createdUserActivity = await _userActivitiesRepository.AddUserActivityAsync(userActivity);
            return Created(createdUserActivity);
        }

        // PUT: odata/UserActivities(UserId=1,ActivityId=1)
        public async Task<IActionResult> Put([FromODataUri] int keyUserId, [FromODataUri] int keyActivityId, [FromBody] UserActivity userActivity)
        {
            if (keyUserId.ToString() != userActivity.UserId || keyActivityId != userActivity.ActivityId)
            {
                return BadRequest();
            }
            await _userActivitiesRepository.UpdateUserActivityAsync(userActivity);
            return NoContent();
        }

        // DELETE: odata/UserActivities(UserId=1,ActivityId=1)
        public async Task<IActionResult> Delete([FromODataUri] string keyUserId, [FromODataUri] int keyActivityId)
        {
            await _userActivitiesRepository.DeleteUserActivityAsync(keyUserId, keyActivityId);
            return NoContent();
        }
    }
}
