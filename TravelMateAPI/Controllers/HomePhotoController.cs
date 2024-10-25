using BusinessObjects.Entities;
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

        // GET: api/HomePhoto
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var homePhotos = await _homePhotoRepository.GetAllHomePhotosAsync();
            return Ok(homePhotos);
        }

        // GET: api/HomePhoto/1
        [HttpGet("{photoId}")]
        public async Task<IActionResult> GetById(int photoId)
        {
            var homePhoto = await _homePhotoRepository.GetHomePhotoByIdAsync(photoId);
            if (homePhoto == null)
            {
                return NotFound(new { Message = $"HomePhoto with PhotoId {photoId} not found." });
            }
            return Ok(homePhoto);
        }

        // POST: api/HomePhoto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HomePhoto newHomePhoto)
        {
            if (newHomePhoto == null)
            {
                return BadRequest("HomePhoto is null.");
            }

            var createdHomePhoto = await _homePhotoRepository.AddHomePhotoAsync(newHomePhoto);
            return CreatedAtAction(nameof(GetById), new { photoId = createdHomePhoto.PhotoId }, createdHomePhoto);
        }

        // PUT: api/HomePhoto/1
        [HttpPut("{photoId}")]
        public async Task<IActionResult> Update(int photoId, [FromBody] HomePhoto updatedHomePhoto)
        {
            if (photoId != updatedHomePhoto.PhotoId)
            {
                return BadRequest("Photo ID mismatch.");
            }

            await _homePhotoRepository.UpdateHomePhotoAsync(updatedHomePhoto);
            return NoContent();
        }

        // DELETE: api/HomePhoto/1
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> Delete(int photoId)
        {
            var homePhoto = await _homePhotoRepository.GetHomePhotoByIdAsync(photoId);
            if (homePhoto == null)
            {
                return NotFound(new { Message = $"HomePhoto with PhotoId {photoId} not found." });
            }

            await _homePhotoRepository.DeleteHomePhotoAsync(photoId);
            return NoContent();
        }
    }

}
