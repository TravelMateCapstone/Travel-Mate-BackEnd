using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;
using Repositories.Interface;
using System.Security.Claims;

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
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
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
        // POST: api/EventParticipants/current-user
        [HttpPost("current-user-join-event")]
        public async Task<IActionResult> AddParticipantForCurrentUser([FromBody] EventParticipants newParticipant)
        {
            if (newParticipant == null)
            {
                return BadRequest("Participant data is null.");
            }

            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Gán UserId của người dùng hiện tại cho participant
            newParticipant.UserId = userId;

            // Thêm người tham gia vào sự kiện
            await _eventParticipantsRepository.AddEventParticipantAsync(newParticipant);

            return Ok(new
            {
                Success = true,
                Message = "Participant added successfully!",
                Data = newParticipant
            });
        }

        // DELETE: api/EventParticipants/1/1
        [HttpDelete("{eventId}/{userId}")]
        public async Task<IActionResult> Remove(int eventId, int userId)
        {
            await _eventParticipantsRepository.RemoveEventParticipantAsync(eventId, userId);
            return NoContent();
        }
        // DELETE: api/EventParticipants/current-user/{eventId}
        [HttpDelete("current-user-out-event/{eventId}")]
        public async Task<IActionResult> RemoveCurrentUserFromEvent(int eventId)
        {
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Xóa người tham gia khỏi sự kiện dựa vào eventId và userId
            await _eventParticipantsRepository.RemoveEventParticipantAsync(eventId, userId);
            return NoContent();
        }
        // GET: api/EventParticipants/event/{eventId}/count
        [HttpGet("event/{eventId}/count-user-join")]
        public async Task<IActionResult> GetParticipantCountByEventId(int eventId)
        {
            // Gọi phương thức từ repository để đếm số lượng người tham gia sự kiện
            var participantCount = await _eventParticipantsRepository.GetParticipantCountByEventIdAsync(eventId);

            if (participantCount == 0)
            {
                return NotFound(new { Message = $"No participants found for EventId {eventId}." });
            }

            return Ok(new { EventId = eventId, ParticipantCount = participantCount });
        }

    }

}
