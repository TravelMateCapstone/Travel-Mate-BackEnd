using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using BusinessObjects.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using TravelMateAPI.Services.ProfileService;

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
        private readonly ApplicationDBContext _context;
        private readonly CheckProfileService _checkProfileService;
        public ProfileController(IProfileRepository profileRepository, UserManager<ApplicationUser> userManager, ApplicationDBContext context,CheckProfileService checkProfileService)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
            _context = context;
            _checkProfileService = checkProfileService;
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

        // GET: api/Profile/current-user/image
        [HttpGet("current-user/image")]
        public async Task<IActionResult> GetCurrentUserImage()
        {
            // Lấy UserId của người dùng hiện tại từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Token không hợp lệ hoặc người dùng không tìm thấy.");
            }

            // Truy vấn để lấy ảnh Profile theo UserId
            var imageUser = await _context.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ImageUser)
                .FirstOrDefaultAsync();

            // Kiểm tra nếu không tìm thấy ảnh
            if (string.IsNullOrEmpty(imageUser))
            {
                imageUser = "https://img.freepik.com/premium-vector/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-vector-illustration_561158-3467.jpg";
            }

            // Trả về giá trị ImageUser hoặc ảnh mặc định
            return Ok(new { imageUser });
        }
        // GET: api/Profile/current-user/image
        [HttpGet("image/{userId}")]
        public async Task<IActionResult> GetUserImage(int userId)
        {
            // Lấy UserId của người dùng hiện tại từ token
            //var userId = GetUserId();
            //if (userId == -1)
            //{
            //    return Unauthorized("Token không hợp lệ hoặc người dùng không tìm thấy.");
            //}

            // Truy vấn để lấy ảnh Profile theo UserId
            var imageUser = await _context.Profiles
                .Where(p => p.UserId == userId)
                .Select(p => p.ImageUser)
                .FirstOrDefaultAsync();

            // Kiểm tra nếu không tìm thấy ảnh
            if (string.IsNullOrEmpty(imageUser))
            {
                imageUser = "https://img.freepik.com/premium-vector/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-vector-illustration_561158-3467.jpg";
            }

            // Trả về giá trị ImageUser hoặc ảnh mặc định
            return Ok(new { imageUser });
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
            //return CreatedAtAction(nameof(GetById), new { profileId = createdProfile.ProfileId }, createdProfile);
            // Return thông báo thành công khi tạo mới
            return Ok(new
            {
                Success = true,
                Message = "Profile created successfully!",
                Data = createdProfile
            });
        }
        // POST: api/Profile/current-user
        [HttpPost("current-user")]
        public async Task<IActionResult> CreateProfileForCurrentUser([FromBody] Profile newProfile)
        {
            if (newProfile == null)
            {
                return BadRequest("Profile is null.");
            }

            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }
            // Kiểm tra xem profile của user đã tồn tại chưa
            var existingProfile = await _profileRepository.GetProfileByIdAsync(userId);
            if (existingProfile != null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Profile for this user already exists."
                });
            }
            // Gán UserId vào profile mới
            newProfile.UserId = userId;

            // Kiểm tra nếu ImageUser không được cung cấp, gán hình ảnh mặc định
            if (string.IsNullOrWhiteSpace(newProfile.ImageUser))
            {
                newProfile.ImageUser = "https://img.freepik.com/premium-vector/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-vector-illustration_561158-3467.jpg";
            }

            // Thêm profile mới vào database
            var createdProfile = await _profileRepository.AddProfileAsync(newProfile);

            // Return thông báo thành công khi tạo mới
            return Ok(new
            {
                Success = true,
                Message = "Profile created successfully!",
                Data = createdProfile
            });
        }

        // PUT: api/Profile/1
        [HttpPut("{profileId}")]
        public async Task<IActionResult> Update(int profileId, [FromBody] Profile updatedProfile)
        {
            if (profileId != updatedProfile.ProfileId)
            {
                return BadRequest("Profile ID mismatch.");
            }

            await _profileRepository.UpdateProfileAsync(updatedProfile);
            return NoContent();
        }
        // PUT: api/Profile/current-user
        [HttpPut("edit-by-current-user")]
        public async Task<IActionResult> UpdateProfileForCurrentUser([FromBody] Profile updatedProfile)
        {
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy profile hiện tại của người dùng từ database
            var existingProfile = await _profileRepository.GetProfileByIdAsync(userId);
            if (existingProfile == null)
            {
                return NotFound(new { Message = $"Profile with UserId {userId} not found." });
            }

            // Kiểm tra nếu profile không thuộc về người dùng hiện tại
            if (existingProfile.UserId != userId)
            {
                return Forbid("You do not have permission to edit this profile.");
            }

            // Cập nhật các thuộc tính của profile hiện tại

            existingProfile.FirstName = updatedProfile.FirstName;
            existingProfile.LastName = updatedProfile.LastName;
            existingProfile.Address = updatedProfile.Address;
            existingProfile.Phone = updatedProfile.Phone;
            existingProfile.Gender = updatedProfile.Gender;
            existingProfile.Birthdate = updatedProfile.Birthdate;
            existingProfile.City = updatedProfile.City;
            existingProfile.Description = updatedProfile.Description;
            existingProfile.HostingAvailability = updatedProfile.HostingAvailability;
            existingProfile.WhyUseTravelMate = updatedProfile.WhyUseTravelMate;
            existingProfile.MusicMoviesBooks = updatedProfile.MusicMoviesBooks;
            existingProfile.WhatToShare = updatedProfile.WhatToShare;
            existingProfile.ImageUser = updatedProfile.ImageUser;

            // Lưu thay đổi
            await _profileRepository.UpdateProfileAsync(existingProfile);

            return Ok(new
            {
                Success = true,
                Message = "Profile updated successfully!",
                Data = existingProfile
            });
        }
        // PUT: api/Profile/current-user/update-image
        [HttpPut("current-user/update-image")]
        public async Task<IActionResult> UpdateProfileImageForCurrentUser([FromBody] string imageUser)
        {
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy profile hiện tại của người dùng từ database
            var existingProfile = await _profileRepository.GetProfileByIdAsync(userId);
            if (existingProfile == null)
            {
                return NotFound(new { Message = $"Profile with UserId {userId} not found." });
            }

            // Kiểm tra nếu profile không thuộc về người dùng hiện tại
            if (existingProfile.UserId != userId)
            {
                return Forbid("You do not have permission to edit this profile.");
            }

            // Cập nhật chỉ thuộc tính ImageUser
            existingProfile.ImageUser = imageUser;

            // Lưu thay đổi
            await _profileRepository.UpdateProfileAsync(existingProfile);

            return Ok(new
            {
                Success = true,
                Message = "Profile image updated successfully!",
                Data = existingProfile
            });
        }


        // DELETE: api/Profile/1
        [HttpDelete("{profileId}")]
        public async Task<IActionResult> Delete(int profileId)
        {
            var profile = await _profileRepository.GetProfileByIdAsync(profileId);
            if (profile == null)
            {
                return NotFound(new { Message = $"Profile with ProfileId {profileId} not found." });
            }

            await _profileRepository.DeleteProfileAsync(profileId);
            return NoContent();
        }

        [HttpGet("checkComplete/{userId}")]
        public async Task<IActionResult> CheckComplete(int userId)
        {
            try
            {
               
                var result = await _checkProfileService.CheckProfileCompletion(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error occurred while checking profile completion.",
                    Error = ex.Message
                });
            }
        }
    }

}
