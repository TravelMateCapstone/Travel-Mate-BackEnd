using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TourParticipantController : ControllerBase
    {
        private readonly ITourParticipantRepository _tourParticipantRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IScheduler _scheduler;

        public TourParticipantController(UserManager<ApplicationUser> userManager, IMapper mapper, ITourParticipantRepository tourParticipantRepository, IScheduler scheduler)
        {
            _tourParticipantRepository = tourParticipantRepository;
            _mapper = mapper;
            _userManager = userManager;
            _scheduler = scheduler;
        }

        //get user (traveler) transaction
        [HttpGet("transactionList")]
        public async Task<ActionResult> GetTransactionList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _tourParticipantRepository.GetTransactionList(user.Id);

            return Ok(result);
        }

        //traveler cancel tour
        [HttpPost("cancelTour")]
        public async Task<ActionResult> CancelTour([FromBody] JoinTourRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var scheduleId = request.ScheduleId;
            var tourId = request.TourId;
            var existingTour = await _tourParticipantRepository.GetTourScheduleById(scheduleId, tourId);
            if (existingTour == null)
                return NotFound();

            var tourSchedule = existingTour.Schedules.FirstOrDefault(t => t.ScheduleId == scheduleId);

            var participant = tourSchedule.Participants.FirstOrDefault(t => t.ParticipantId == user.Id);

            if (participant.PaymentStatus != BusinessObjects.EnumClass.PaymentStatus.Success)
                return BadRequest("Access Denied! You are not in tour");

            var timeNow = GetTimeZone.GetVNTimeZoneNow().Date;
            if (tourSchedule.StartDate.Date <= timeNow && timeNow <= tourSchedule.EndDate.Date)
                return BadRequest("Access Denied! Tour is on going!");

            if (tourSchedule.EndDate.Date < timeNow)
                return BadRequest("Access Denied! Tour've already done!");

            if ((tourSchedule.StartDate - timeNow).TotalDays <= 2)
                return BadRequest("Access Denied! You cannot cancel the tour within 2 days of its scheduled start.");

            //update 
            participant.PaymentStatus = BusinessObjects.EnumClass.PaymentStatus.ProcessRefund;

            await _tourParticipantRepository.UpdateRefundStatus(existingTour, tourSchedule.ScheduleId, user.Id);

            return Ok();
        }

        //deactive tour
        [HttpPost("changeTourStatus")]
        public async Task<ActionResult> ChangeTourStatus([FromBody] JoinTourRequest request, [FromQuery] bool isActive)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var scheduleId = request.ScheduleId;
            var tourId = request.TourId;
            var existingTour = await _tourParticipantRepository.GetTourScheduleById(scheduleId, tourId);
            if (existingTour == null)
                return NotFound();

            var tourSchedule = existingTour.Schedules.FirstOrDefault(t => t.ScheduleId == scheduleId);

            //check thoi gian da dien ra, dang dien ra
            var timeNow = GetTimeZone.GetVNTimeZoneNow().Date;
            if (tourSchedule.StartDate.Date <= timeNow && timeNow <= tourSchedule.EndDate.Date)
                return BadRequest("Access Denied! Tour is on going!");

            if (tourSchedule.EndDate.Date < timeNow)
                return BadRequest("Access Denied! Tour've already done!");

            if (tourSchedule.Participants.Count > 0)
                return BadRequest("Access Denied! Tour has participants");

            await _tourParticipantRepository.ProcessTourStatus(scheduleId, tourId, isActive);

            return Ok();
        }

        [HttpPost("tourParticipants")]
        public async Task<ActionResult<IEnumerable<Participants>>> GetListParticipantsAsync([FromBody] JoinTourRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scheduleId = request.ScheduleId;
            var tourId = request.TourId;
            var listParticipants = await _tourParticipantRepository.GetListParticipantsAsync(scheduleId, tourId);

            return Ok(listParticipants);
        }

        [HttpPost("join")]
        public async Task<ActionResult> JoinTour([FromBody] JoinTourRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var scheduleId = request.ScheduleId;
            var tourId = request.TourId;
            var existingTour = await _tourParticipantRepository.GetTourScheduleById(scheduleId, tourId);
            if (existingTour == null)
                return NotFound();

            if (existingTour.ApprovalStatus != BusinessObjects.EnumClass.ApprovalStatus.Accepted)
                return BadRequest("Access Denied! Tour is not public");

            var jobKey = new JobKey($"{user.Id}", "group1");
            if (await _scheduler.CheckExists(jobKey))
            {
                return BadRequest("Access Denied! Finish your tour booking process before booking another tour");
            }

            var tourSchedule = existingTour.Schedules.FirstOrDefault(t => t.ScheduleId == scheduleId);

            var timeNow = GetTimeZone.GetVNTimeZoneNow().Date;
            if (tourSchedule.StartDate.Date <= timeNow && timeNow <= tourSchedule.EndDate.Date)
                return BadRequest("Access Denied! Tour is on going!");

            if (tourSchedule.EndDate.Date < timeNow)
                return BadRequest("Access Denied! Tour've already done!");

            if (tourSchedule.Participants.Count == existingTour.MaxGuests)
                return BadRequest("No available slots in this tour");

            if (tourSchedule.Participants.Any(t => t.ParticipantId == user.Id))
                return BadRequest("You have already joined this tour .");

            if (tourSchedule.ActiveStatus == false)
                return BadRequest("Tour is not active");

            if (existingTour.Creator.Id == user.Id)
                return BadRequest("Access Denied! You are creator of this tour");

            await _tourParticipantRepository.JoinTour(scheduleId, tourId, user.Id);

            return Ok("Join tour successful");
        }
    }
}
