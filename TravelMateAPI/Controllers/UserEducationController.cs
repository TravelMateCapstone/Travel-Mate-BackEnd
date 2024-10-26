using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserEducationController : ControllerBase
    {
        private readonly IUserEducationRepository _userEducationRepository;

        public UserEducationController(IUserEducationRepository userEducationRepository)
        {
            _userEducationRepository = userEducationRepository;
        }

        // GET: api/UserEducation
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userEducations = await _userEducationRepository.GetAllUserEducationsAsync();
            return Ok(userEducations);
        }
        // GET: api/UserEducation/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var userEducation = await _userEducationRepository.GetUserEducationByUserIdAsync(userId);
            if (userEducation == null || !userEducation.Any())
            {
                return NotFound(new { Message = $"No education found for UserId {userId}." });
            }
            return Ok(userEducation);
        }
        // GET: api/UserEducation/1/1
        [HttpGet("{universityId}/{userId}")]
        public async Task<IActionResult> GetById(int universityId, int userId)
        {
            var userEducation = await _userEducationRepository.GetUserEducationByIdAsync(universityId, userId);
            if (userEducation == null)
            {
                return NotFound(new { Message = $"UserEducation with UniversityId {universityId} and UserId {userId} not found." });
            }
            return Ok(userEducation);
        }

        // POST: api/UserEducation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserEducation newUserEducation)
        {
            if (newUserEducation == null)
            {
                return BadRequest("UserEducation is null.");
            }

            var createdUserEducation = await _userEducationRepository.AddUserEducationAsync(newUserEducation);
            return CreatedAtAction(nameof(GetById), new { universityId = createdUserEducation.UniversityId, userId = createdUserEducation.UserId }, createdUserEducation);
        }

        // PUT: api/UserEducation/1/1
        [HttpPut("{universityId}/{userId}")]
        public async Task<IActionResult> Update(int universityId, int userId, [FromBody] UserEducation updatedUserEducation)
        {
            if (universityId != updatedUserEducation.UniversityId || userId != updatedUserEducation.UserId)
            {
                return BadRequest("UserEducation ID mismatch.");
            }

            await _userEducationRepository.UpdateUserEducationAsync(updatedUserEducation);
            return NoContent();
        }

        // DELETE: api/UserEducation/1/1
        [HttpDelete("{universityId}/{userId}")]
        public async Task<IActionResult> Delete(int universityId, int userId)
        {
            var userEducation = await _userEducationRepository.GetUserEducationByIdAsync(universityId, userId);
            if (userEducation == null)
            {
                return NotFound(new { Message = $"UserEducation with UniversityId {universityId} and UserId {userId} not found." });
            }

            await _userEducationRepository.DeleteUserEducationAsync(universityId, userId);
            return NoContent();
        }
    }

}
