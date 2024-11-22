using AutoMapper;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivitiesWOOController : ControllerBase
    {
        private readonly IUserActivitiesRepository _userActivityRepository;
        private readonly IMapper _mapper;
        public UserActivitiesWOOController(IUserActivitiesRepository userActivitiesRepository, IMapper mapper)
        {
            _userActivityRepository = userActivitiesRepository;
            _mapper = mapper;
        }
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        // GET: api/UserActivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserActivity>>> GetUserActivities()
        {
            var userActivities = await _userActivityRepository.GetAllUserActivitiesAsync();
            var userActivityDTOs = _mapper.Map<List<UserActivityDTO>>(userActivities);
            return Ok(userActivityDTOs);
        }
        // GET: api/UserActivity
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var userActivities = await _userActivityRepository.GetAllUserActivitiesAsync();
        //    return Ok(userActivities);
        //}

        // GET: api/UserActivity/1/1
        [HttpGet("{userId}/{activityId}")]
        public async Task<IActionResult> GetById(int userId, int activityId)
        {
            var userActivity = await _userActivityRepository.GetUserActivityByIdAsync(userId, activityId);
            if (userActivity == null)
            {
                return NotFound(new { Message = $"UserActivity with UserId {userId} and ActivityId {activityId} not found." });
            }
            return Ok(userActivity);
        }
        // GET: api/UserActivity/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var userActivities = await _userActivityRepository.GetUserActivitiesByUserIdAsync(userId);
            //if (userActivities == null || !userActivities.Any())
            //{
            //    return NotFound(new { Message = $"No activities found for UserId {userId}." });
            //}
            return Ok(userActivities);
        }
        // GET: api/UserActivities
        [HttpGet("current-user")]
        public async Task<ActionResult<IEnumerable<UserActivity>>> GetUserActivitiesCurrent()
        {
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            var userActivities = await _userActivityRepository.GetUserActivitiesByUserIdAsync(userId);
            var userActivityDTOs = _mapper.Map<List<UserActivityDTO>>(userActivities);
            return Ok(userActivityDTOs);
        }
        // POST: api/UserActivity
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserActivity newUserActivity)
        {
            if (newUserActivity == null)
            {
                return BadRequest("UserActivity is null.");
            }

            var createdUserActivity = await _userActivityRepository.AddUserActivityAsync(newUserActivity);
            return CreatedAtAction(nameof(GetById), new { userId = createdUserActivity.UserId, activityId = createdUserActivity.ActivityId }, createdUserActivity);
        }
        // POST: api/UserActivity
        [HttpPost("edit-by-current-user")]
        public async Task<IActionResult> CreateCurrentUser([FromBody] UserActivity newUserActivity)
        {
            if (newUserActivity == null)
            {
                return BadRequest("UserActivity is null.");
            }

            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Gán UserId vào hoạt động mới
            newUserActivity.UserId = userId;

            var createdUserActivity = await _userActivityRepository.AddUserActivityAsync(newUserActivity);
            return CreatedAtAction(nameof(GetById), new { userId = createdUserActivity.UserId, activityId = createdUserActivity.ActivityId }, createdUserActivity);
        }
        // PUT: api/UserActivity/1/1
        [HttpPut("{userId}/{activityId}")]
        public async Task<IActionResult> Update(int userId, int activityId, [FromBody] UserActivity updatedUserActivity)
        {
            if (userId != updatedUserActivity.UserId || activityId != updatedUserActivity.ActivityId)
            {
                return BadRequest("UserActivity ID mismatch.");
            }

            await _userActivityRepository.UpdateUserActivityAsync(updatedUserActivity);
            return NoContent();
        }

        // DELETE: api/UserActivity/1/1
        [HttpDelete("{userId}/{activityId}")]
        public async Task<IActionResult> Delete(int userId, int activityId)
        {
            var userActivity = await _userActivityRepository.GetUserActivityByIdAsync(userId, activityId);
            if (userActivity == null)
            {
                return NotFound(new { Message = $"UserActivity with UserId {userId} and ActivityId {activityId} not found." });
            }

            await _userActivityRepository.DeleteUserActivityAsync(userId, activityId);
            return NoContent();
        }
        // DELETE: api/UserActivity
        [HttpDelete("current-user/{activityId}")]
        public async Task<IActionResult> Delete(int activityId)
        {
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            var userActivity = await _userActivityRepository.GetUserActivityByIdAsync(userId, activityId);
            if (userActivity == null)
            {
                return NotFound(new { Message = $"UserActivity with UserId {userId} and ActivityId {activityId} not found." });
            }

            await _userActivityRepository.DeleteUserActivityAsync(userId, activityId);
            return NoContent();
        }
    }
}
