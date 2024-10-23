using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    ////[ApiController]
    ////[Route("odata/[controller]")]
    //public class LocationsController : ODataController
    //{
    //    private readonly ILocationRepository _locationRepository;

    //    public LocationsController(ILocationRepository locationRepository)
    //    {
    //        _locationRepository = locationRepository;
    //    }

    //    [EnableQuery]
    //    public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUser> queryOptions)
    //    {
    //        var locations = await _locationRepository.GetAllLocationsAsync();
    //        return Ok(locations);
    //    }

    //    /*public IActionResult GetAll()
    //    {
    //        var locations = _locationRepository.GetAllLocationsAsync().Result.AsQueryable();
    //        return Ok(locations);
    //    }
    //    */
    //    [EnableQuery]
    //    public async Task<IActionResult> Get([FromODataUri] int key)
    //    {
    //        var location = await _locationRepository.GetLocationByIdAsync(key);
    //        if (location == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(location);
    //    }

    //    public async Task<IActionResult> Post([FromBody] Location location)
    //    {
    //        var createdLocation = await _locationRepository.AddLocationAsync(location);
    //        return Created(createdLocation);
    //    }

    //    public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Location location)
    //    {
    //        if (key != location.LocationId)
    //        {
    //            return BadRequest();
    //        }
    //        await _locationRepository.UpdateLocationAsync(location);
    //        return NoContent();
    //    }

    //    public async Task<IActionResult> Delete([FromODataUri] int key)
    //    {
    //        await _locationRepository.DeleteLocationAsync(key);
    //        return NoContent();
    //    }
    //}


    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase 
    {
        private readonly ILocationRepository _locationRepository;

        public LocationsController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationRepository.GetAllLocationsAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int key)
        {
            var location = await _locationRepository.GetLocationByIdAsync(key);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest("Activity is null.");
            }
            var createdLocation = await _locationRepository.AddLocationAsync(location);
            return CreatedAtAction(nameof(GetById), new { id = createdLocation.LocationId }, createdLocation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Location location)
        {
            if (id != location.LocationId)
            {
                return BadRequest("Activity ID mismatch.");
            }
            await _locationRepository.UpdateLocationAsync(location);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _locationRepository.GetLocationByIdAsync(id);
            if (activity == null)
            {
                return NotFound(new { Message = $"Activity with id {id} not found." });
            }
            await _locationRepository.DeleteLocationAsync(id);
            return NoContent();
        }
    }
}
