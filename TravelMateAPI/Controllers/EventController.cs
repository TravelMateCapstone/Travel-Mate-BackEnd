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
    public class EventController : ODataController
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: odata/Events
        [EnableQuery] // Kích hoạt OData cho truy vấn
        public IActionResult GetAll()
        {
            var events = _eventRepository.GetAllEventsAsync().Result.AsQueryable();
            return Ok(events);
        }

        // GET: odata/Events(1)
        [EnableQuery] // Kích hoạt OData cho truy vấn theo ID
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var eventObj = await _eventRepository.GetEventByIdAsync(key);
            if (eventObj == null)
            {
                return NotFound();
            }
            return Ok(eventObj);
        }

        // POST: odata/Events
        public async Task<IActionResult> Post([FromBody] Event eventObj)
        {
            var createdEvent = await _eventRepository.AddEventAsync(eventObj);
            return Created(createdEvent);
        }

        // PUT: odata/Events(1)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Event eventObj)
        {
            if (key != eventObj.EventId)
            {
                return BadRequest();
            }
            await _eventRepository.UpdateEventAsync(eventObj);
            return NoContent();
        }

        // DELETE: odata/Events(1)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            await _eventRepository.DeleteEventAsync(key);
            return NoContent();
        }
    }

}
