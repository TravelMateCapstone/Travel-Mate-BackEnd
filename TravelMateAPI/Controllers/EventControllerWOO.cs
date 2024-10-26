using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventControllerWOO : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventControllerWOO(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/Event
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return Ok(events);
        }

        // GET: api/Event/creater/1
        [HttpGet("creater/{createrUserId}")]
        public async Task<IActionResult> GetByCreaterUserId(int createrUserId)
        {
            var events = await _eventRepository.GetEventsByCreaterUserIdAsync(createrUserId);
            if (events == null || !events.Any())
            {
                return NotFound(new { Message = $"No events found for CreaterUserId {createrUserId}." });
            }
            return Ok(events);
        }

        // GET: api/Event/1
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetById(int eventId)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound(new { Message = $"Event with Id {eventId} not found." });
            }
            return Ok(eventItem);
        }

        // POST: api/Event
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event is null.");
            }

            var createdEvent = await _eventRepository.AddEventAsync(newEvent);
            return CreatedAtAction(nameof(GetById), new { eventId = createdEvent.EventId }, createdEvent);
        }

        // PUT: api/Event/1
        [HttpPut("{eventId}")]
        public async Task<IActionResult> Update(int eventId, [FromBody] Event updatedEvent)
        {
            if (eventId != updatedEvent.EventId)
            {
                return BadRequest("EventId mismatch.");
            }

            await _eventRepository.UpdateEventAsync(updatedEvent);
            return NoContent();
        }

        // DELETE: api/Event/1
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> Delete(int eventId)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound(new { Message = $"Event with Id {eventId} not found." });
            }

            await _eventRepository.DeleteEventAsync(eventId);
            return NoContent();
        }
    }

}
