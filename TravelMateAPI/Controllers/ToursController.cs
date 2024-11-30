﻿using AutoMapper;
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
        public int travelerId = 9;

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

            var getUserInfo = await _tourRepository.GetUserInfo(userId);

            //map tour creator with user
            tour.Creator.Fullname = getUserInfo.FullName;
            tour.Creator.AvatarUrl = getUserInfo.Profiles.ImageUser;
            tour.Creator.Address = getUserInfo.Profiles.City;
            tour.Creator.Rating = 4;
            tour.Creator.TotalTrips = 10;
            tour.Creator.JoinedAt = getUserInfo.RegistrationTime;

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

            var existingTour = await _tourRepository.GetTourById(id);
            if (existingTour == null)
                return NotFound();

            if (userId != existingTour.Creator.Id)
                return BadRequest("You are not creator of this tour!");

            var tour = _mapper.Map<Tour>(tourDto);

            await _tourRepository.UpdateTour(id, tour);
            return NoContent();
        }

        // DELETE: api/tour/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTour(string id)
        {
            var existingTour = await _tourRepository.GetTourById(id);

            if (existingTour == null)
                return NotFound();

            if (existingTour.Creator.Id != userId)
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

            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            if (existingTour.Creator.Id == travelerId)
                return BadRequest("Access Denied! You are creator of this tour");

            await _tourRepository.JoinTour(tourId, travelerId);
            return Ok("Join tour successful");
        }

        // POST: api/tour/accept/{tourId}
        [HttpPost("accept/{tourId}")]
        public async Task<ActionResult> AcceptTour(string tourId)
        {
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            await _tourRepository.AcceptTour(tourId);
            return NoContent();
        }

        [HttpPost("ban/{tourId}")]
        public async Task<ActionResult> BanTour(string tourId)
        {
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();

            await _tourRepository.BanTour(tourId);
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
            //user thuoc trong participant
            var existingTour = await _tourRepository.GetTourById(tourId);
            if (existingTour == null)
                return NotFound();
            var existingParticipant = await _tourRepository.DoesParticipantExist(userId);

            if (!existingParticipant)
                return BadRequest("You are not in this tour");

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
