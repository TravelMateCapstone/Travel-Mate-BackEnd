using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interface;
using System.Security.Claims;
using TravelMateAPI.Services;
using TravelMateAPI.Services.Notification;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventParticipantsController : ControllerBase
    {
        private readonly IEventParticipantsRepository _eventParticipantsRepository;
        private readonly INotificationService _notificationService;
        private readonly ApplicationDBContext _context;

        public EventParticipantsController(IEventParticipantsRepository eventParticipantsRepository,INotificationService notificationService,ApplicationDBContext context)
        {
            _eventParticipantsRepository = eventParticipantsRepository;
            _notificationService = notificationService;
            _context = context;
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
        [HttpGet("top-events")]
        public async Task<IActionResult> GetTopEvents(int topCount)
        {
           // const int topCount = 5; // Số lượng sự kiện cần lấy
            var topEvents = await _eventParticipantsRepository.GetTopEventsByParticipantCountAsync(topCount);

            if (topEvents == null || !topEvents.Any())
            {
                return NotFound(new { Message = "Không tìm thấy sự kiện nào có người tham gia." });
            }

            return Ok(topEvents);
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

            // Lấy thông tin sự kiện để kiểm tra hoặc lấy thêm dữ liệu
            var eventDetails = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == newParticipant.EventId);
            int toUserId = eventDetails.CreaterUserId;

            var userDetail = await _context.Users.FindAsync(userId);

            // Gán UserId của người dùng hiện tại cho participant
            newParticipant.UserId = userId;
            // Gán giá trị mặc định cho JoinedAt và Notification
            newParticipant.JoinedAt = GetTimeZone.GetVNTimeZoneNow();
            newParticipant.Notification = true;


            // Thêm người tham gia vào sự kiện
            await _eventParticipantsRepository.AddEventParticipantAsync(newParticipant);

            await _notificationService.CreateNotificationFullAsync(toUserId, $"{userDetail.FullName} đã tham gia sự kiện [{eventDetails.EventName}] của bạn.", eventDetails.EventId, 3);

            return Ok(new
            {
                Success = true,
                Message = "Bạn đã tham gia sự kiện thành công",
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

        // GET: api/EventParticipants/current-user/has-joined/{eventId}
        [HttpGet("check-current-user-joined/{eventId}")]
        public async Task<IActionResult> HasCurrentUserJoinedEvent(int eventId)
        {
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Kiểm tra xem người dùng hiện tại đã tham gia sự kiện hay chưa
            var hasJoined = await _eventParticipantsRepository.HasUserJoinedEventAsync(eventId, userId);

            return Ok(hasJoined);
        }

    }

}
