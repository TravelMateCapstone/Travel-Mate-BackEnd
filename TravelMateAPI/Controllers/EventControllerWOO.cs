using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;

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
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
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
        // POST: api/Event/current-user
        [HttpPost("add-by-current-user")]
        public async Task<IActionResult> CreateEventForCurrentUser([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event is null.");
            }

            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Gán UserId vào CreaterUserId của sự kiện
            newEvent.CreaterUserId = userId;

            // Thêm sự kiện mới vào cơ sở dữ liệu
            var createdEvent = await _eventRepository.AddEventAsync(newEvent);

            // Trả về phản hồi thành công
            return CreatedAtAction(nameof(GetById), new { eventId = createdEvent.EventId }, createdEvent);
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
        // PUT: api/Event/current-user/1
        [HttpPut("edit-by-current-user/{eventId}")]
        public async Task<IActionResult> UpdateEventForCurrentUser(int eventId, [FromBody] Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                return BadRequest("Event is null.");
            }

            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Tìm sự kiện cần cập nhật
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound(new { Message = $"Event with Id {eventId} not found." });
            }

            // Kiểm tra nếu sự kiện không phải của người dùng hiện tại
            if (eventItem.CreaterUserId != userId)
            {
                return Forbid("You are not the owner of this event.");
            }

            // Cập nhật thông tin sự kiện
            updatedEvent.EventId = eventId; // Đảm bảo cập nhật đúng sự kiện
            updatedEvent.CreaterUserId = userId; // Đảm bảo không thay đổi người tạo sự kiện

            await _eventRepository.UpdateEventAsync(updatedEvent);
            return NoContent();
        }
        // DELETE: api/Event/current-user/1
        [HttpDelete("current-user-delete-event/{eventId}")]
        public async Task<IActionResult> DeleteEventForCurrentUser(int eventId)
        {
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Tìm sự kiện cần xóa
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound(new { Message = $"Event with Id {eventId} not found." });
            }

            // Kiểm tra nếu sự kiện không phải do người dùng hiện tại tạo
            if (eventItem.CreaterUserId != userId)
            {
                return Forbid("You are not the creator of this event.");
            }

            // Xóa sự kiện nếu quyền hợp lệ
            await _eventRepository.DeleteEventAsync(eventId);
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
