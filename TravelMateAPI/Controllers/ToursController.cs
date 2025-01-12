using AutoMapper;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects.Utils.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using Repository.Interfaces;

namespace TravelMate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourRepository _tourRepository;
        private readonly IPastTripPostRepository _tripRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TourController(UserManager<ApplicationUser> userManager, IMapper mapper, ITourRepository tourRepository, IPastTripPostRepository tripRepository)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
            _userManager = userManager;
            _tripRepository = tripRepository;
        }

        // GET: api/tour
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourDto>>> GetAllTours()
        {
            var tours = await _tourRepository.GetAllTours();

            var tourDto = _mapper.Map<IEnumerable<TourDto>>(tours);

            return Ok(tourDto);
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
        public async Task<ActionResult<IEnumerable<TourBriefDto>>> GetToursByStatus(string approvalStatus)
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

            var tourDto = _mapper.Map<IEnumerable<TourBriefDto>>(tours);

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
            var star = await _tripRepository.GetUserAverageStar(tour.Creator.Id);
            var trip = await _tripRepository.GetUserTotalTrip(tour.Creator.Id);

            tour.Creator.Fullname = creatorInfo.FullName;
            tour.Creator.AvatarUrl = creatorInfo.Profiles.ImageUser;
            tour.Creator.Address = creatorInfo.Profiles.City;
            tour.Creator.JoinedAt = creatorInfo.RegistrationTime;
            tour.Creator.Rating = star;
            tour.Creator.TotalTrips = trip;
            //await _tourRepository.UpdateTour(tour.TourId, tour);

            var tourDto = _mapper.Map<TourDto>(tour);
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

            if (existingTour.ApprovalStatus == ApprovalStatus.Accepted)
                return BadRequest("Access Denied! Tour was already public!");

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

        [Authorize(Roles = "admin")]
        // POST: api/tour/accept/{tourId}
        [HttpPost("accept/{tourId}")]
        public async Task<ActionResult> AcceptTour(string tourId)
        {
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            if (existingTour.ApprovalStatus != ApprovalStatus.Pending)
                return BadRequest("You have processed this tour request");

            await _tourRepository.AcceptTour(tourId);
            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("reject/{tourId}")]
        public async Task<ActionResult> RejectTour(string tourId)
        {
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            if (existingTour.ApprovalStatus != ApprovalStatus.Pending)
                return BadRequest("You have processed this tour request");

            await _tourRepository.RejectTour(tourId);
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

            if (existingTour.ApprovalStatus == ApprovalStatus.Accepted)
                return BadRequest("Access Denied! Tour is public!");

            await _tourRepository.CancelTour(tourId);
            return NoContent();
        }
    }
}
