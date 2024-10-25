using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

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

        // GET: api/UserHome
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userHomes = await _userHomeRepository.GetAllUserHomesAsync();
            return Ok(userHomes);
        }

        // GET: api/UserHome/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var userHome = await _userHomeRepository.GetUserHomeByIdAsync(userId);
            if (userHome == null)
            {
                return NotFound(new { Message = $"UserHome with UserId {userId} not found." });
            }
            return Ok(userHome);
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
            return CreatedAtAction(nameof(GetById), new { userId = createdUserHome.UserId }, createdUserHome);
        }

        // PUT: api/UserHome/1
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(int userId, [FromBody] UserHome updatedUserHome)
        {
            if (userId != updatedUserHome.UserId)
            {
                return BadRequest("UserHome ID mismatch.");
            }

            await _userHomeRepository.UpdateUserHomeAsync(updatedUserHome);
            return NoContent();
        }

        // DELETE: api/UserHome/1
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var userHome = await _userHomeRepository.GetUserHomeByIdAsync(userId);
            if (userHome == null)
            {
                return NotFound(new { Message = $"UserHome with UserId {userId} not found." });
            }

            await _userHomeRepository.DeleteUserHomeAsync(userId);
            return NoContent();
        }
    }

}
