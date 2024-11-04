using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventControllerWOO : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ApplicationDBContext _context;
        private readonly IEventParticipantsRepository _eventParticipantsRepository;

        public EventControllerWOO(IEventRepository eventRepository, ApplicationDBContext context, IEventParticipantsRepository eventParticipantsRepository)
        {
            _eventRepository = eventRepository;
            _context = context;
            _eventParticipantsRepository = eventParticipantsRepository;
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
        // GET: api/Event/my-events
        [HttpGet("get-event-current-user")]
        public async Task<IActionResult> GetMyEvents()
        {
            // Lấy UserId từ JWT token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized(new { Message = "Invalid token or user not found." });
            }

            // Gọi repository để lấy danh sách sự kiện của user
            var events = await _eventRepository.GetEventsByCreaterUserIdAsync(userId);
            if (events == null || !events.Any())
            {
                return NotFound(new { Message = $"No events found for the current user with UserId {userId}." });
            }

            return Ok(events);
        }
        // GET: api/Events/{eventId}/participants
        [HttpGet("{eventId}/Event-With-Profiles-join")]
        public async Task<IActionResult> GetEventParticipantsWithProfiles(int eventId)
        {
            // Lấy danh sách người tham gia sự kiện kèm theo thông tin Profile
            var participantsWithProfiles = await _context.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .Select(ep => new
                {
                    ep.UserId,
                    ep.EventId,
                    ep.JoinedAt,
                    Profile = _context.Profiles.FirstOrDefault(p => p.UserId == ep.UserId) // Lấy thông tin Profile từ UserId
                })
                .ToListAsync();

            // Nếu không có người tham gia nào
            if (!participantsWithProfiles.Any())
            {
                return NotFound(new { Message = "No participants found for this event." });
            }

            return Ok(participantsWithProfiles);
        }
        // GET: api/Events/user/joined
        [HttpGet("user/joined")]
        public async Task<IActionResult> GetJoinedEventsForCurrentUser()
        {
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy các sự kiện mà người dùng đã tham gia
            var joinedEvents = await _context.Events
                .Where(e => _context.EventParticipants
                    .Any(ep => ep.EventId == e.EventId && ep.UserId == userId))
                .ToListAsync();

            return Ok(joinedEvents);
        }

        // GET: api/Events/user/not-joined
        [HttpGet("user/not-joined")]
        public async Task<IActionResult> GetNotJoinedEventsForCurrentUser()
        {
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy các sự kiện mà người dùng chưa tham gia
            var notJoinedEvents = await _context.Events
                .Where(e => !_context.EventParticipants
                    .Any(ep => ep.EventId == e.EventId && ep.UserId == userId))
                .ToListAsync();
            //// Lấy các sự kiện mà người dùng chưa tham gia và cũng không phải là người tạo ra sự kiện
            //var notJoinedEvents = await _context.Events
            //    .Where(e => !_context.EventParticipants
            //                    .Any(ep => ep.EventId == e.EventId && ep.UserId == userId)
            //                && e.CreaterUserId != userId)
            //    .ToListAsync();

            return Ok(notJoinedEvents);
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
            // Thêm người dùng hiện tại vào danh sách tham gia sự kiện ngay lập tức
            var newParticipant = new EventParticipants
            {
                EventId = createdEvent.EventId,
                UserId = userId,
                JoinedAt = DateTime.Now,
                Notification = true
            };
            await _eventParticipantsRepository.AddEventParticipantAsync(newParticipant);
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

            // Cập nhật thông tin sự kiện (chỉ các trường được phép cập nhật)
            eventItem.EventName = updatedEvent.EventName;
            eventItem.Description = updatedEvent.Description;
            eventItem.EventImageUrl = updatedEvent.EventImageUrl;
            eventItem.StartAt = updatedEvent.StartAt;
            eventItem.EndAt = updatedEvent.EndAt;
            eventItem.EventLocation = updatedEvent.EventLocation;

            await _eventRepository.UpdateEventAsync(updatedEvent);
            return Ok(new
            {
                Success = true,
                Message = "Event updated successfully!",
                Data = eventItem
            });
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
