using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersWOOController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;

        public ApplicationUsersWOOController(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("/GetUsersWithProfilesAndRoles")]
        public async Task<IActionResult> GetUsersWithProfilesAndRoles()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }
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

    }
}
