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
    public class UserLocationsWOOController : Controller
    {
        private readonly IUserLocationsRepository _userLocationRepository;
        private readonly IMapper _mapper;
        public UserLocationsWOOController(IUserLocationsRepository userLocationsRepository, IMapper mapper)
        {
            _userLocationRepository = userLocationsRepository;
            _mapper = mapper;
        }
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        // GET: api/UserLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLocation>>> GetUserLocations()
        {
            var userLocations = await _userLocationRepository.GetAllUserLocationsAsync();
            var userLocationDTOs = _mapper.Map<List<UserLocationDTO>>(userLocations);
            return Ok(userLocationDTOs);
        }
        // GET: api/UserLocation/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var userLocations = await _userLocationRepository.GetUserLocationsByUserIdAsync(userId);
            if (userLocations == null || !userLocations.Any())
            {
                return NotFound(new { Message = $"No locations found for UserId {userId}." });
            }
            return Ok(userLocations);
        }
        // GET: api/UserLocation
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var userLocations = await _userLocationRepository.GetAllUserLocationsAsync();
        //    return Ok(userLocations);
        //}
        // GET: api/UserLocation/user/current
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetByCurrentUserId()
        {
            var userId = GetUserId(); // Giả sử bạn đã có phương thức này để lấy UserId từ token.

            var userLocations = await _userLocationRepository.GetUserLocationsByUserIdAsync(userId);
            if (userLocations == null || !userLocations.Any())
            {
                return NotFound(new { Message = $"No locations found for UserId {userId}." });
            }
            return Ok(userLocations);
        }
        // GET: api/UserLocation/1/1
        [HttpGet("{userId}/{locationId}")]
        public async Task<IActionResult> GetById(int userId, int locationId)
        {
            var userLocation = await _userLocationRepository.GetUserLocationByIdAsync(userId, locationId);
            if (userLocation == null)
            {
                return NotFound(new { Message = $"UserLocation with UserId {userId} and LocationId {locationId} not found." });
            }
            return Ok(userLocation);
        }

        // POST: api/UserLocation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserLocation newUserLocation)
        {
            if (newUserLocation == null)
            {
                return BadRequest("UserLocation is null.");
            }

            var createdUserLocation = await _userLocationRepository.AddUserLocationAsync(newUserLocation);
            return CreatedAtAction(nameof(GetById), new { userId = createdUserLocation.UserId, locationId = createdUserLocation.LocationId }, createdUserLocation);
        }
        // POST: api/UserLocation
        [HttpPost("post-curent-user")]
        public async Task<IActionResult> CreatebyCurrentUser([FromBody] UserLocation newUserLocation)
        {
            if (newUserLocation == null)
            {
                return BadRequest("UserLocation is null.");
            }

            // Lấy UserId hiện tại
            newUserLocation.UserId = GetUserId(); // Giả sử bạn có phương thức GetUserId để lấy UserId từ token.

            var createdUserLocation = await _userLocationRepository.AddUserLocationAsync(newUserLocation);
            return CreatedAtAction(nameof(GetById), new { userId = createdUserLocation.UserId, locationId = createdUserLocation.LocationId }, createdUserLocation);
        }
        // PUT: api/UserLocation/1/1
        [HttpPut("{userId}/{locationId}")]
        public async Task<IActionResult> Update(int userId, int locationId, [FromBody] UserLocation updatedUserLocation)
        {
            if (userId != updatedUserLocation.UserId || locationId != updatedUserLocation.LocationId)
            {
                return BadRequest("UserLocation ID mismatch.");
            }

            await _userLocationRepository.UpdateUserLocationAsync(updatedUserLocation);
            return NoContent();
        }

        // DELETE: api/UserLocation/1/1
        [HttpDelete("{userId}/{locationId}")]
        public async Task<IActionResult> Delete(int userId, int locationId)
        {
            var userLocation = await _userLocationRepository.GetUserLocationByIdAsync(userId, locationId);
            if (userLocation == null)
            {
                return NotFound(new { Message = $"UserLocation with UserId {userId} and LocationId {locationId} not found." });
            }

            await _userLocationRepository.DeleteUserLocationAsync(userId, locationId);
            return NoContent();
        }

        // DELETE: api/UserLocation/user/{locationId}
        [HttpDelete("delete-current-user/{locationId}")]
        public async Task<IActionResult> DeleteForUser(int locationId)
        {
            var userId = GetUserId(); // Giả sử bạn có phương thức GetUserId để lấy UserId từ token.

            // Lấy thông tin UserLocation theo locationId
            var userLocation = await _userLocationRepository.GetUserLocationByIdAsync(userId, locationId);
            if (userLocation == null)
            {
                return NotFound(new { Message = $"UserLocation with LocationId {locationId} not found for UserId {userId}." });
            }

            // Kiểm tra xem UserId có phải là người tạo UserLocation hay không
            if (userLocation.UserId != userId)
            {
                return Forbid("You are not authorized to delete this UserLocation.");
            }

            await _userLocationRepository.DeleteUserLocationAsync(userId, locationId);
            return NoContent();
        }
    }
}
