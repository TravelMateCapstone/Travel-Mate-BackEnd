using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("odata/[controller]")]
    public class UserLocationsController : ODataController
    {
        private readonly IUserLocationsRepository _userLocationsRepository;

        public UserLocationsController(IUserLocationsRepository userLocationsRepository)
        {
            _userLocationsRepository = userLocationsRepository;
        }

        // GET: odata/UserLocations
        [EnableQuery] // Cho phép OData query
        public IActionResult GetAll()
        {
            var userLocations = _userLocationsRepository.GetAllUserLocationsAsync().Result.AsQueryable();
            return Ok(userLocations);
        }

        // GET: odata/UserLocations(UserId=1,LocationId=1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] string keyUserId, [FromODataUri] int keyLocationId)
        {
            var userLocation = await _userLocationsRepository.GetUserLocationByIdAsync(keyUserId, keyLocationId);
            if (userLocation == null)
            {
                return NotFound();
            }
            return Ok(userLocation);
        }

        // POST: odata/UserLocations
        public async Task<IActionResult> Post([FromBody] UserLocation userLocation)
        {
            var createdUserLocation = await _userLocationsRepository.AddUserLocationAsync(userLocation);
            return Created(createdUserLocation);
        }

        // PUT: odata/UserLocations(UserId=1,LocationId=1)
        public async Task<IActionResult> Put([FromODataUri] int keyUserId, [FromODataUri] int keyLocationId, [FromBody] UserLocation userLocation)
        {
            if (keyUserId.ToString() != userLocation.UserId || keyLocationId != userLocation.LocationId)
            {
                return BadRequest();
            }
            await _userLocationsRepository.UpdateUserLocationAsync(userLocation);
            return NoContent();
        }

        // DELETE: odata/UserLocations(UserId=1,LocationId=1)
        public async Task<IActionResult> Delete([FromODataUri] string keyUserId, [FromODataUri] int keyLocationId)
        {
            await _userLocationsRepository.DeleteUserLocationAsync(keyUserId, keyLocationId);
            return NoContent();
        }
    }
}
