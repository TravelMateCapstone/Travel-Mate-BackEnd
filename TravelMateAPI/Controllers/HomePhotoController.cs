using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomePhotoController : ControllerBase
    {
        private readonly IHomePhotoRepository _homePhotoRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomePhotoController(IHomePhotoRepository homePhotoRepository, UserManager<ApplicationUser> userManager)
        {
            _homePhotoRepository = homePhotoRepository;
            _userManager = userManager;
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

        // POST: api/HomePhoto/user/current
        [HttpPost("user/current")]
        public async Task<IActionResult> AddHomePhotoForCurrentUser([FromBody] string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                return BadRequest("Photo URL is required.");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found.");
            }

            try
            {
                var addedPhoto = await _homePhotoRepository.AddHomePhotoByUserIdAsync(currentUser.Id, photoUrl);
                return CreatedAtAction(nameof(GetPhotosByUserId), new { userId = addedPhoto.UserHomeId }, addedPhoto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("currentAddImagesHome")]
        public async Task<IActionResult> AddHomePhotosForCurrentUser([FromBody] AddHomePhotosRequest request)
        {
            if (request.PhotoUrls == null || request.PhotoUrls.Count == 0)
            {
                return BadRequest("At least one photo URL is required.");
            }

            // Lấy người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found.");
            }

            try
            {
                // Kiểm tra UserHomeId có hợp lệ hay không
                var userHomeId = request.UserHomeId;
                if (userHomeId <= 0)
                {
                    return BadRequest("Invalid UserHomeId.");
                }

                // Tạo danh sách các đối tượng HomePhoto từ danh sách URL ảnh
                var homePhotos = request.PhotoUrls.Select(photoUrl => new HomePhoto
                {
                    UserHomeId = userHomeId, // Gán UserHomeId từ request
                    HomePhotoUrl = photoUrl // Gán URL ảnh vào HomePhotoUrl
                }).ToList();

                // Thêm tất cả các ảnh vào cơ sở dữ liệu trong một lần
                await _homePhotoRepository.AddHomePhotosAsync(homePhotos);

                // Trả về phản hồi thành công với tất cả ảnh đã thêm
                return Ok(new { Message = "Photos added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/HomePhoto/{photoId}
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeleteHomePhoto(int photoId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found.");
            }
            await _homePhotoRepository.DeleteHomePhotoAsync(photoId);
            return Ok("Ảnh được xóa thành công");
        }
    }
}