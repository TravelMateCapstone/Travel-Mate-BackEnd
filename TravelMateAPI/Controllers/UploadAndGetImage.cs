using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using TravelMateAPI.Services.Firebase;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadAndGetImage : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly FirebaseService _firebaseService;
        public UploadAndGetImage(IProfileRepository profileRepository, FirebaseService firebaseService)
        {
            _profileRepository = profileRepository;
            _firebaseService = firebaseService;
        }
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

