using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesControllerWOO : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        public ProfilesControllerWOO(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        // GET: api/Profile
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles = await _profileRepository.GetAllProfilesAsync();
            return Ok(profiles);
        }

        // GET: api/Profile/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var profile = await _profileRepository.GetProfileByIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { Message = $"Profile with UserId {userId} not found." });
            }
            return Ok(profile);
        }

        // POST: api/Profile
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Profile newProfile)
        {
            if (newProfile == null)
            {
                return BadRequest("Profile is null.");
            }

            var createdProfile = await _profileRepository.AddProfileAsync(newProfile);
            return CreatedAtAction(nameof(GetById), new { userId = createdProfile.UserId }, createdProfile);
        }

        // PUT: api/Profile/1
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(int userId, [FromBody] Profile updatedProfile)
        {
            if (userId != updatedProfile.UserId)
            {
                return BadRequest("Profile ID mismatch.");
            }

            await _profileRepository.UpdateProfileAsync(updatedProfile);
            return NoContent();
        }

        // DELETE: api/Profile/1
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var profile = await _profileRepository.GetProfileByIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { Message = $"Profile with UserId {userId} not found." });
            }

            await _profileRepository.DeleteProfileAsync(userId);
            return NoContent();
        }
    }
}
