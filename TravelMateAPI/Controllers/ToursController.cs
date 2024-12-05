using AutoMapper;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects.Utils.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TourController(UserManager<ApplicationUser> userManager, IMapper mapper, ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: api/tour
        //GET all tour in db
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourDto>>> GetAllTours()
        {
            var tours = await _tourRepository.GetAllTours();

            var tourDto = _mapper.Map<IEnumerable<TourDto>>(tours);

            return Ok(tourDto);
        }

        [HttpGet("tourParticipants")]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetListParticipantsAsync(string tourId)
        {
            var listParticipants = await _tourRepository.GetListParticipantsAsync(tourId);

            return Ok(listParticipants);
        }

        //get all tour of a local
        [HttpGet("local/{userId}")]
        public async Task<ActionResult<IEnumerable<TourDto>>> GetAllToursOfLocal(int userId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tours = await _tourRepository.GetAllToursOfLocal(userId);

            var tourDto = _mapper.Map<IEnumerable<TourDto>>(tours);

            return Ok(tourDto);
        }

        [HttpGet("toursStatus/{approvalStatus}")]
        public async Task<ActionResult<IEnumerable<TourDto>>> GetToursByStatus(string approvalStatus)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!Enum.TryParse<ApprovalStatus>(approvalStatus, true, out var status))
            {
                return BadRequest("Invalid status");
            }

            var tours = await _tourRepository.GetToursByStatus(user.Id, status);

            var tourDto = _mapper.Map<IEnumerable<TourDto>>(tours);

            return Ok(tourDto);
        }

        // GET: api/tour/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TourDto>> GetTourById(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tour = await _tourRepository.GetTourById(id);
            if (tour == null)
            {
                return NotFound();
            }

            var creatorInfo = await _tourRepository.GetUserInfo(tour.Creator.Id);

            tour.Creator.Fullname = creatorInfo.FullName;
            tour.Creator.AvatarUrl = creatorInfo.Profiles.ImageUser;
            tour.Creator.Address = creatorInfo.Profiles.City;
            tour.Creator.Rating = await _tourRepository.GetUserAverageStar(tour.Creator.Id);
            tour.Creator.TotalTrips = await _tourRepository.GetUserTotalTrip(tour.Creator.Id);
            tour.Creator.JoinedAt = creatorInfo.RegistrationTime;

            await _tourRepository.UpdateTour(tour.TourId, tour);

            var tourDto = _mapper.Map<TourDto>(tour);
            //return local info
            return Ok(tourDto);
        }

        // POST: api/tour
        [HttpPost]
        public async Task<ActionResult> AddTour([FromBody] TourDto tourDto)
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

            var tour = _mapper.Map<Tour>(tourDto);

            await _tourRepository.AddTour(user.Id, tour);
            return Ok(tour);
        }

        // PUT: api/tour/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTour(string id, [FromBody] TourDto tourDto)
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

            var existingTour = await _tourRepository.GetTourById(id);
            if (existingTour == null)
                return NotFound();

            if (user.Id != existingTour.Creator.Id)
                return BadRequest("You are not creator of this tour!");

            var tour = _mapper.Map<Tour>(tourDto);

            await _tourRepository.UpdateTour(id, tour);
            return NoContent();
        }

        // DELETE: api/tour/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTour(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existingTour = await _tourRepository.GetTourById(id);

            if (existingTour == null)
                return NotFound();

            if (existingTour.Creator.Id != user.Id)
                return BadRequest("Access Denied! You are not tour creator");

            await _tourRepository.DeleteTour(id);
            return NoContent();
        }

        // POST: api/tour/join/{tourId}
        [HttpPost("join/{tourId}")]
        public async Task<ActionResult> JoinTour(string tourId)
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

            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            if (existingTour.Participants.Count == existingTour.MaxGuests)
                return BadRequest("No available slots in this tour");

            var doesUserExistInTour = await _tourRepository.DoesParticipantExist(tourId, user.Id);
            if (doesUserExistInTour)
                return BadRequest("You have joined this tour");

            if (existingTour.Creator.Id == user.Id)
                return BadRequest("Access Denied! You are creator of this tour");

            await _tourRepository.JoinTour(tourId, user.Id);
            return Ok("Join tour successful");
        }

        // POST: api/tour/accept/{tourId}
        [HttpPost("accept/{tourId}")]
        public async Task<ActionResult> AcceptTour(string tourId)
        {
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            if (existingTour.ApprovalStatus != ApprovalStatus.Pending)
                return BadRequest("You have processed this tour request");

            //neu da co trang thai roi thi ko dc accept nua

            await _tourRepository.AcceptTour(tourId);
            return NoContent();
        }

        [HttpPost("reject/{tourId}")]
        public async Task<ActionResult> RejectTour(string tourId)
        {
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            //neu da co trang thai roi thi ko dc accept nua
            if (existingTour.ApprovalStatus != ApprovalStatus.Pending)
                return BadRequest("You have processed this tour request");

            await _tourRepository.RejectTour(tourId);
            return NoContent();
        }

        // POST: api/tour/review/{tourId}
        [HttpPost("review/{tourId}")]
        public async Task<ActionResult> AddReview(string tourId, [FromBody] TourReview tourReview)
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

            //user thuoc trong participant
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            var existingParticipant = await _tourRepository.DoesParticipantExist(tourId, user.Id);

            if (!existingParticipant)
                return BadRequest("You are not in this tour");

            await _tourRepository.AddReview(tourId, tourReview);
            return NoContent();
        }

        // PUT: api/tour/cancel/{tourId}
        [HttpPut("cancel/{tourId}")]
        public async Task<ActionResult> CancelTour(string tourId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            if (existingTour.Creator.Id != user.Id)
                return BadRequest("Access Denied! You are not creator of this tour");

            await _tourRepository.CancelTour(tourId);
            return NoContent();
        }
    }
}
