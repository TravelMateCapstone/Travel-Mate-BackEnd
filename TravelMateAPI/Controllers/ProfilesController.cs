using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using BusinessObjects.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TravelMateAPI.Controllers
{
    //    //[ApiController]
    //    //[Route("odata/[controller]")]
    //    public class ProfilesController : ODataController
    //    {
    //        private readonly IProfileRepository _profileRepository;
    //        private readonly FirebaseService _firebaseService;
    //        public ProfilesController(IProfileRepository profileRepository, FirebaseService firebaseService)
    //        {
    //            _profileRepository = profileRepository;
    //            _firebaseService = firebaseService;
    //        }

    //        // GET: odata/Profiles
    //        [EnableQuery] // Cho phép OData query
    //        public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUser> queryOptions)
    //        {
    //            var profiles = _profileRepository.GetAllProfilesAsync();
    //            return Ok(profiles);
    //        }

    //        /*public IActionResult GetAll()
    //        {
    //            var profiles = _profileRepository.GetAllProfilesAsync().Result.AsQueryable();
    //            return Ok(profiles);
    //        }*/

    //        // GET: odata/Profiles(1)
    //        [EnableQuery] // Cho phép OData truy vấn theo UserId
    //        public async Task<IActionResult> Get([FromODataUri] int key)
    //        {
    //            var profile = await _profileRepository.GetProfileByIdAsync(key);
    //            if (profile == null)
    //            {
    //                return NotFound();
    //            }
    //            return Ok(profile);
    //        }

    //        // POST: odata/Profiles
    //        public async Task<IActionResult> Post([FromBody] Profile profile)
    //        {
    //            var createdProfile = await _profileRepository.AddProfileAsync(profile);
    //            return Created(createdProfile);
    //        }

    //        // PUT: odata/Profiles(1)
    //        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Profile profile)
    //        {
    //            if (key != profile.UserId)
    //            {
    //                return BadRequest();
    //            }
    //            await _profileRepository.UpdateProfileAsync(profile);
    //            return NoContent();
    //        }

    //        // DELETE: odata/Profiles(1)
    //        public async Task<IActionResult> Delete([FromODataUri] int key)
    //        {
    //            await _profileRepository.DeleteProfileAsync(key);
    //            return NoContent();
    //        }

    //        POST: odata/Profiles/UploadImage
    //       [HttpPost("UploadImage")]
    //public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] int userId) // Đảm bảo userId là string
    //        {
    //            if (file == null || file.Length == 0)
    //                return BadRequest("File cannot be empty");

    //            try
    //            {
    //                // Upload file lên Firebase
    //                var imageUrl = await _firebaseService.UploadFileAsync(file, userId);

    //                // Lấy thông tin Profile của người dùng
    //                var profile = await _profileRepository.GetProfileByIdAsync(userId);
    //                if (profile == null)
    //                {
    //                    return NotFound($"Profile with UserId '{userId}' not found.");
    //                }s

    //                // Cập nhật URL của hình ảnh vào Profile
    //                profile.ImageUser = imageUrl;
    //                await _profileRepository.UpdateProfileAsync(profile);

    //                return Ok(new { imageUrl });
    //            }
    //            catch (Exception ex)
    //            {
    //                return StatusCode(500, $"Internal server error: {ex.Message}");
    //            }
    //        }

    //        // API để lấy imageURL theo UserId
    //        [HttpGet("GetImageUrl/{userId}")]
    //        public async Task<IActionResult> GetImageUrl(int userId)
    //        {
    //            // Lấy thông tin Profile dựa trên UserId
    //            var profile = await _profileRepository.GetProfileByIdAsync(userId);

    //            if (profile == null)
    //            {
    //                return NotFound($"Profile with UserId '{userId}' not found.");
    //            }

    //            // Kiểm tra nếu imageURL có tồn tại
    //            if (string.IsNullOrEmpty(profile.ImageUser))
    //            {
    //                return NotFound("No image URL found for this user.");
    //            }

    //            // Trả về URL của hình ảnh
    //            return Ok(new { imageUrl = profile.ImageUser });
    //        }
    //    }
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileController(IProfileRepository profileRepository, UserManager<ApplicationUser> userManager)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
        }
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
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
            // Kiểm tra xem userId có tồn tại trong hệ thống hay không
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound(new { Message = $"UserId {userId} not found." });
            }
            var profile = await _profileRepository.GetProfileByIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { Message = $"Profile with UserId {userId} null." });
            }
            return Ok(profile);
        }
        // API để lấy profile của người dùng hiện tại
        [HttpGet("current-profile")]
        public async Task<IActionResult> GetCurrentProfile()
        {
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Tìm hồ sơ của người dùng dựa vào UserId
            var profile = await _profileRepository.GetProfileByIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { Message = $"Profile with UserId {userId} null" });
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
