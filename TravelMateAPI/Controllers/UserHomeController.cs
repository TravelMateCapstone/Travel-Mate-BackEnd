using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserHomeController : ControllerBase
    {
        private readonly IUserHomeRepository _userHomeRepository;

        public UserHomeController(IUserHomeRepository userHomeRepository)
        {
            _userHomeRepository = userHomeRepository;
        }
        // Lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        // GET: api/UserHome
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userHomes = await _userHomeRepository.GetAllUserHomesAsync();
            return Ok(userHomes);
        }

        // GET: api/UserHome/1
        [HttpGet("{userHomeId}")]
        public async Task<IActionResult> GetById(int userHomeId)
        {
            var userHome = await _userHomeRepository.GetUserHomeByIdAsync(userHomeId);
            if (userHome == null)
            {
                return NotFound(new { Message = $"UserHome with UserHomeId {userHomeId} not found." });
            }
            return Ok(userHome);
        }

        // GET: api/UserHome/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var userHome = await _userHomeRepository.GetUserHomeByUserIdAsync(userId);
            if (userHome == null)
            {
                return NotFound(new { Message = $"UserHome with UserId {userId} not found." });
            }
            return Ok(userHome);
        }
        // GET: api/UserHome/user
        [HttpGet("current-user")]
        public async Task<IActionResult> GetAllByUserId()
        {
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            var userHomes = await _userHomeRepository.GetUserHomeByUserIdAsync(userId);
            if (userHomes == null )
            {
                return NotFound(new { Message = $"No homes found for UserId {userId}." });
            }
            return Ok(userHomes);
        }
        // POST: api/UserHome
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserHome newUserHome)
        {
            if (newUserHome == null)
            {
                return BadRequest("UserHome is null.");
            }

            var createdUserHome = await _userHomeRepository.AddUserHomeAsync(newUserHome);
            return CreatedAtAction(nameof(GetById), new { userHomeId = createdUserHome.UserHomeId }, createdUserHome);
        }
        // POST: api/UserHome/user
        [HttpPost("by-current-user")]
        public async Task<IActionResult> CreateByUserId([FromBody] UserHome newUserHome)
        {
            if (newUserHome == null)
            {
                return BadRequest("UserHome is null.");
            }

            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            newUserHome.UserId = userId; // Gán UserId từ token

            var createdUserHome = await _userHomeRepository.AddUserHomeAsync(newUserHome);
            return CreatedAtAction(nameof(GetById), new { userHomeId = createdUserHome.UserHomeId }, createdUserHome);
        }
        // PUT: api/UserHome/1
        [HttpPut("{userHomeId}")]
        public async Task<IActionResult> Update(int userHomeId, [FromBody] UserHome updatedUserHome)
        {
            if (userHomeId != updatedUserHome.UserHomeId)
            {
                return BadRequest("UserHome ID mismatch.");
            }

            await _userHomeRepository.UpdateUserHomeAsync(updatedUserHome);
            return NoContent();
        }
        // PUT: api/UserHome/user/1
        [HttpPut("edit-current-user/{userHomeId}")]
        public async Task<IActionResult> UpdateForUser(int userHomeId, [FromBody] UserHome updatedUserHome)
        {
            if (updatedUserHome == null)
            {
                return BadRequest("UserHome is null.");
            }

            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy thông tin UserHome theo userHomeId
            var existingUserHome = await _userHomeRepository.GetUserHomeByIdAsync(userHomeId);
            if (existingUserHome == null)
            {
                return NotFound(new { Message = $"UserHome with UserHomeId {userHomeId} not found." });
            }

            // Kiểm tra xem userId có phải là người tạo UserHome hay không
            if (existingUserHome.UserId != userId)
            {
                return Forbid("You are not authorized to update this UserHome.");
            }

            // Cập nhật thông tin UserHome
            existingUserHome.MaxGuests = updatedUserHome.MaxGuests;
            existingUserHome.GuestPreferences = updatedUserHome.GuestPreferences;
            existingUserHome.AllowedSmoking = updatedUserHome.AllowedSmoking;
            existingUserHome.RoomType = updatedUserHome.RoomType;
            existingUserHome.RoomDescription = updatedUserHome.RoomDescription;
            existingUserHome.RoomMateInfo = updatedUserHome.RoomMateInfo;
            existingUserHome.Amenities = updatedUserHome.Amenities;
            existingUserHome.OverallDescription = updatedUserHome.OverallDescription;
            existingUserHome.Transportation = updatedUserHome.Transportation;

            // Nếu bạn cần cập nhật các HomePhotos, cần xử lý riêng
            if (updatedUserHome.HomePhotos != null)
            {
                existingUserHome.HomePhotos = updatedUserHome.HomePhotos; // Cập nhật danh sách HomePhotos
            }

            await _userHomeRepository.UpdateUserHomeAsync(existingUserHome);
            //return NoContent();
            return Ok(new
            {
                Success = true,
                Message = "Home updated successfully!",
                Data = existingUserHome
            });
        }


        // DELETE: api/UserHome/1
        [HttpDelete("{userHomeId}")]
        public async Task<IActionResult> Delete(int userHomeId)
        {
            var userHome = await _userHomeRepository.GetUserHomeByIdAsync(userHomeId);
            if (userHome == null)
            {
                return NotFound(new { Message = $"UserHome with UserHomeId {userHomeId} not found." });
            }

            await _userHomeRepository.DeleteUserHomeAsync(userHomeId);
            return NoContent();
        }

        // DELETE: api/UserHome/user/1
        [HttpDelete("current-user/{userHomeId}")]
        public async Task<IActionResult> DeleteForUser(int userHomeId)
        {
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy thông tin UserHome theo userHomeId
            var userHome = await _userHomeRepository.GetUserHomeByIdAsync(userHomeId);
            if (userHome == null)
            {
                return NotFound(new { Message = $"UserHome with UserHomeId {userHomeId} not found." });
            }

            // Kiểm tra xem userId có phải là người tạo UserHome hay không
            if (userHome.UserId != userId)
            {
                return Forbid("You are not authorized to delete this UserHome.");
            }

            // Xóa UserHome
            await _userHomeRepository.DeleteUserHomeAsync(userHomeId);
            return NoContent();
        }

    }


}
