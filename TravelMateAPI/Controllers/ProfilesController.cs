using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;
using BussinessObjects.Entities;
using TravelMateAPI.Services.Firebase;
using FirebaseAdmin.Messaging;
using Repositories;

namespace TravelMateAPI.Controllers
{
    //[ApiController]
    //[Route("odata/[controller]")]
    public class ProfilesController : ODataController
    {
        private readonly IProfileRepository _profileRepository;
        private readonly FirebaseService _firebaseService;
        public ProfilesController(IProfileRepository profileRepository, FirebaseService firebaseService)
        {
            _profileRepository = profileRepository;
            _firebaseService = firebaseService;
        }

        // GET: odata/Profiles
        [EnableQuery] // Cho phép OData query
        public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUser> queryOptions)
        {
            var profiles = _profileRepository.GetAllProfilesAsync();
            return Ok(profiles);
        }

        /*public IActionResult GetAll()
        {
            var profiles = _profileRepository.GetAllProfilesAsync().Result.AsQueryable();
            return Ok(profiles);
        }*/

        // GET: odata/Profiles(1)
        [EnableQuery] // Cho phép OData truy vấn theo UserId
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var profile = await _profileRepository.GetProfileByIdAsync(key);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        // POST: odata/Profiles
        public async Task<IActionResult> Post([FromBody] Profile profile)
        {
            var createdProfile = await _profileRepository.AddProfileAsync(profile);
            return Created(createdProfile);
        }

        // PUT: odata/Profiles(1)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Profile profile)
        {
            if (key != profile.UserId)
            {
                return BadRequest();
            }
            await _profileRepository.UpdateProfileAsync(profile);
            return NoContent();
        }

        // DELETE: odata/Profiles(1)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            await _profileRepository.DeleteProfileAsync(key);
            return NoContent();
        }

        // POST: odata/Profiles/UploadImage
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] int userId) // Đảm bảo userId là string
        {
            if (file == null || file.Length == 0)
                return BadRequest("File cannot be empty");

            try
            {
                // Upload file lên Firebase
                var imageUrl = await _firebaseService.UploadFileAsync(file, userId);

                // Lấy thông tin Profile của người dùng
                var profile = await _profileRepository.GetProfileByIdAsync(userId);
                if (profile == null)
                {
                    return NotFound($"Profile with UserId '{userId}' not found.");
                }

                // Cập nhật URL của hình ảnh vào Profile
                profile.ImageUser = imageUrl;
                await _profileRepository.UpdateProfileAsync(profile);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // API để lấy imageURL theo UserId
        [HttpGet("GetImageUrl/{userId}")]
        public async Task<IActionResult> GetImageUrl(int userId)
            {
                // Lấy thông tin Profile dựa trên UserId
                var profile = await _profileRepository.GetProfileByIdAsync(userId);

                if (profile == null)
                {
                    return NotFound($"Profile with UserId '{userId}' not found.");
                }

                // Kiểm tra nếu imageURL có tồn tại
                if (string.IsNullOrEmpty(profile.ImageUser))
                {
                    return NotFound("No image URL found for this user.");
                }

                // Trả về URL của hình ảnh
                return Ok(new { imageUrl = profile.ImageUser });
            }
        }
}
