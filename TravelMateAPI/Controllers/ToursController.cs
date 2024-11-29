using AutoMapper;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;
        public int userId = 8;

        public TourController(IMapper mapper, ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
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

        //get all tour of a local
        [HttpGet("local")]
        public async Task<ActionResult<IEnumerable<TourDto>>> GetAllToursOfLocal()
        {
            var tours = await _tourRepository.GetAllToursOfLocal(userId);

            var tourDto = _mapper.Map<IEnumerable<TourDto>>(tours);

            return Ok(tourDto);
        }

        [HttpGet("toursStatus/{approvalStatus}")]
        public async Task<ActionResult<IEnumerable<TourDto>>> GetToursByStatus(string approvalStatus)
        {
            bool? status = approvalStatus.ToLower() switch
            {
                "pending" => null,
                "accepted" => true,
                "rejected" => false,
                _ => throw new ArgumentException("Invalid status")
            };

            var tours = await _tourRepository.GetToursByStatus(userId, status);

            var tourDto = _mapper.Map<IEnumerable<TourDto>>(tours);

            return Ok(tourDto);
        }

        //get all tour of a participant

        // GET: api/tour/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TourDto>> GetTourById(string id)
        {
            var tour = await _tourRepository.GetTourById(id);
            if (tour == null)
            {
                return NotFound();
            }

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

            var tour = _mapper.Map<Tour>(tourDto);

            await _tourRepository.AddTour(userId, tour);
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

            //check if user was creator

            if (userId != tourDto.Creator.Id)
                return BadRequest("You are not creator of this tour!");

            var existingTour = await _tourRepository.GetTourById(id);

            if (existingTour == null)
                return NotFound();

            var tour = _mapper.Map<Tour>(tourDto);

            await _tourRepository.UpdateTour(id, tour);
            return NoContent();
        }

        // DELETE: api/tour/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTour(string id)
        {
            await _tourRepository.DeleteTour(id);
            return NoContent();
        }

        // POST: api/tour/join/{tourId}
        [HttpPost("join/{tourId}")]
        public async Task<ActionResult> JoinTour(string tourId, [FromBody] Participants participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tourRepository.JoinTour(tourId, participant);
            return NoContent();
        }

        // POST: api/tour/accept/{tourId}
        [HttpPost("accept/{tourId}")]
        public async Task<ActionResult> AcceptTour(string tourId)
        {
            await _tourRepository.AcceptTour(tourId);
            return NoContent();
        }

        // POST: api/tour/review/{tourId}
        [HttpPost("review/{tourId}")]
        public async Task<ActionResult> AddReview(string tourId, [FromBody] TourReview tourReview)
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return BadRequest(ModelState); // Return a BadRequest with validation errors
            }

            await _tourRepository.AddReview(tourId, tourReview);
            return NoContent();
        }

        // PUT: api/tour/availability/{tourId}
        [HttpPut("availability/{tourId}")]
        public async Task<ActionResult> UpdateAvailability(string tourId, [FromBody] int slots)
        {
            if (!ModelState.IsValid) // Check if the model state is valid
            {
                return BadRequest(ModelState); // Return a BadRequest with validation errors
            }

            await _tourRepository.UpdateAvailability(tourId, slots);
            return NoContent();
        }

        // PUT: api/tour/cancel/{tourId}
        [HttpPut("cancel/{tourId}")]
        public async Task<ActionResult> CancelTour(string tourId)
        {
            await _tourRepository.CancelTour(tourId);
            return NoContent();
        }
    }
}
