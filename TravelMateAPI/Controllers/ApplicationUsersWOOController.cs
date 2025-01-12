using BusinessObjects.Utils.Request;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;
using TravelMateAPI.Services.FilterLocal;

namespace TravelMateAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersWOOController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly FilterUserService _filterService;

        public ApplicationUsersWOOController(IApplicationUserRepository userRepository, FilterUserService filterService)
        {
            _userRepository = userRepository;
            _filterService = filterService;
        }
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        [Authorize(Roles = "admin")]
        [HttpGet("/GetUsersWithProfilesAndRoles")]
        public async Task<IActionResult> GetUsersWithProfilesAndRoles()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("/GetUsersWithDetail")]
        public async Task<IActionResult> GetUsersWithDetail()
        {
            // Lấy UserId từ JWT token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized(new { Message = "Invalid token or user not found." });
            }

            var users = await _filterService.GetAllUsersWithDetailsAsync(userId);
            return Ok(users);
        }

        [HttpGet("/GetUsersWithDetail-byRole/{role}")]
        public async Task<IActionResult> GetUsersWithDetailByRole(string role)
        {
            var users = await _filterService.GetAllUsersWithDetailsByRoleAsync(role);
            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Lấy thông tin người dùng từ repository
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            // Xóa người dùng
            _userRepository.DeleteUser(user);

            return Ok(new
            {
                Success = true,
                Message = $"User with ID {id} has been deleted."
            });
        }
        [Authorize(Roles = "admin")]
        [HttpPut("ban/{id}")]
        public async Task<IActionResult> BanUser(int id)
        {
            // Lấy thông tin người dùng từ repository
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            // Cập nhật trạng thái người dùng
            user.LockoutEnabled = true; // Đánh dấu người dùng bị khóa
            user.LockoutEnd = DateTimeOffset.MaxValue; // Đặt thời gian hết hạn khóa là vô hạn
            _userRepository.UpdateUser(user);

            return Ok(new
            {
                Success = true,
                Message = $"User with ID {id} has been banned."
            });
        }

        [HttpPut("update-fullname")]
        public async Task<IActionResult> UpdateFullName( [FromBody] UpdateFullNameRequest request)
        {
            try
            {
                // Lấy UserId từ token của người dùng hiện tại
                var userId = GetUserId();
                if (userId == -1)
                {
                    return Unauthorized("Invalid token or user not found.");
                }

                // Lấy thông tin user theo Id
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { Message = "Người dùng không tồn tại." });
                }

                // Cập nhật FullName
                user.FullName = request.FullName;

                // Lưu thay đổi
                _userRepository.UpdateUser(user);

                return Ok(new
                {
                    Success = true,
                    Message = "Cập nhật FullName thành công.",
                    Data = new
                    {
                        user.Id,
                        user.FullName
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
