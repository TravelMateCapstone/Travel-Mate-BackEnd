using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;
using BussinessObjects.Entities;
using Repositories;
using AutoMapper;
using BussinessObjects.Utils.Reponse;
using Sprache;

namespace TravelMateAPI.Controllers
{
    //[ApiController]
    //[Route("odata/[controller]")]
    public class UserActivitiesController : ODataController
    {
        private readonly IUserActivitiesRepository _userActivitiesRepository;
        private readonly IMapper _mapper;
        public UserActivitiesController(IUserActivitiesRepository userActivitiesRepository, IMapper mapper)
        {
            _userActivitiesRepository = userActivitiesRepository;
            _mapper = mapper;
        }

        // GET: odata/UserActivities
        [EnableQuery]
        public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUser> queryOptions)
        {
            var userActivities = _userActivitiesRepository.GetAllUserActivitiesAsync().Result.AsQueryable(); ;
            var userActivityDTOs = _mapper.Map<List<UserActivityDTO>>(userActivities);
            //return Ok(userActivities);
            return Ok(userActivityDTOs.AsQueryable());
        }

        /*public IActionResult GetAll()
        {
            var userActivities = _userActivitiesRepository.GetAllUserActivitiesAsync().Result.AsQueryable();
            return Ok(userActivities);
        }*/

        // GET: odata/UserActivities(UserId=1,ActivityId=1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int keyUserId, [FromODataUri] int keyActivityId)
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
            if (keyUserId != userActivity.UserId || keyActivityId != userActivity.ActivityId)
            {
                return BadRequest();
            }
            await _userActivitiesRepository.UpdateUserActivityAsync(userActivity);
            return NoContent();
        }

        // DELETE: odata/UserActivities(UserId=1,ActivityId=1)
        public async Task<IActionResult> Delete([FromODataUri] int keyUserId, [FromODataUri] int keyActivityId)
        {
            await _userActivitiesRepository.DeleteUserActivityAsync(keyUserId, keyActivityId);
            return NoContent();
        }
    }

}
