using AutoMapper;
using BussinessObjects.Entities;
using BussinessObjects.Utils.Reponse;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLocationsWOOController : ControllerBase
    {
        private readonly IUserLocationsRepository _userLocationRepository;
        private readonly IMapper _mapper;
        public UserLocationsWOOController(IUserLocationsRepository userLocationsRepository, IMapper mapper)
        {
            _userLocationRepository = userLocationsRepository;
            _mapper = mapper;
        }

        // GET: api/UserLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLocation>>> GetUserLocations()
        {
            var userLocations = await _userLocationRepository.GetAllUserLocationsAsync();
            var userLocationDTOs = _mapper.Map<List<UserLocationDTO>>(userLocations);
            return Ok(userLocationDTOs);
        }
        //// GET: api/UserLocation
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var userLocations = await _userLocationRepository.GetAllUserLocationsAsync();
        //    return Ok(userLocations);
        //}

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
    }
}
