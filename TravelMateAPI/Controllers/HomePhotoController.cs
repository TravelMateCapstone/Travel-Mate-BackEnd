﻿using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomePhotoController : ControllerBase
    {
        private readonly IHomePhotoRepository _homePhotoRepository;

        public HomePhotoController(IHomePhotoRepository homePhotoRepository)
        {
            _homePhotoRepository = homePhotoRepository;
        }

        // GET: api/HomePhoto/home/{userHomeId}
        [HttpGet("home/{userHomeId}")]
        public async Task<IActionResult> GetPhotosByHomeId(int userHomeId)
        {
            var photos = await _homePhotoRepository.GetPhotosByHomeIdAsync(userHomeId);
            if (photos == null || photos.Count == 0)
            {
                return NotFound(new { Message = $"No photos found for UserHomeId {userHomeId}" });
            }
            return Ok(photos);
        }

        // POST: api/HomePhoto
        [HttpPost]
        public async Task<IActionResult> AddHomePhoto([FromBody] HomePhoto newHomePhoto)
        {
            if (newHomePhoto == null)
            {
                return BadRequest("HomePhoto is null.");
            }

            var addedPhoto = await _homePhotoRepository.AddHomePhotoAsync(newHomePhoto);
            return CreatedAtAction(nameof(GetPhotosByHomeId), new { userHomeId = addedPhoto.UserHomeId }, addedPhoto);
        }

        // GET: api/HomePhoto/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPhotosByUserId(int userId)
        {
            var photos = await _homePhotoRepository.GetPhotosByUserIdAsync(userId);
            if (photos == null || photos.Count == 0)
            {
                return NotFound(new { Message = $"No photos found for UserId {userId}" });
            }
            return Ok(photos);
        }

        // POST: api/HomePhoto/user/{userId}
        [HttpPost("user/{userId}")]
        public async Task<IActionResult> AddHomePhotoByUserId(int userId, [FromBody] string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                return BadRequest("Photo URL is required.");
            }

            try
            {
                var addedPhoto = await _homePhotoRepository.AddHomePhotoByUserIdAsync(userId, photoUrl);
                return CreatedAtAction(nameof(GetPhotosByUserId), new { userId = addedPhoto.UserHomeId }, addedPhoto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
