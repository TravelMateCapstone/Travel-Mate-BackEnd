using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventParticipantsController : ControllerBase
    {
        private readonly IEventParticipantsRepository _eventParticipantsRepository;

        public EventParticipantsController(IEventParticipantsRepository eventParticipantsRepository)
        {
            _eventParticipantsRepository = eventParticipantsRepository;
        }

        // GET: api/EventParticipants
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var participants = await _eventParticipantsRepository.GetAllEventParticipantsAsync();
            return Ok(participants);
        }

        // GET: api/EventParticipants/event/1
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEventId(int eventId)
        {
            var participants = await _eventParticipantsRepository.GetEventParticipantsByEventIdAsync(eventId);
            if (participants == null || !participants.Any())
            {
                return NotFound(new { Message = $"No participants found for EventId {eventId}." });
            }
            return Ok(participants);
        }

        // POST: api/EventParticipants
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EventParticipants newParticipant)
        {
            if (newParticipant == null)
            {
                return BadRequest("Participant data is null.");
            }

            await _eventParticipantsRepository.AddEventParticipantAsync(newParticipant);
            return Ok(newParticipant);
        }

        // DELETE: api/EventParticipants/1/1
        [HttpDelete("{eventId}/{userId}")]
        public async Task<IActionResult> Remove(int eventId, int userId)
        {
            await _eventParticipantsRepository.RemoveEventParticipantAsync(eventId, userId);
            return NoContent();
        }
    }

}
