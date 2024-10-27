using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpokenLanguagesController : ControllerBase
    {
        private readonly ISpokenLanguagesRepository _spokenLanguagesRepository;

        public SpokenLanguagesController(ISpokenLanguagesRepository spokenLanguagesRepository)
        {
            _spokenLanguagesRepository = spokenLanguagesRepository;
        }
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        // GET: api/SpokenLanguages
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var spokenLanguages = await _spokenLanguagesRepository.GetAllSpokenLanguagesAsync();
            return Ok(spokenLanguages);
        }
        // GET: api/SpokenLanguage/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var spokenLanguages = await _spokenLanguagesRepository.GetSpokenLanguagesByUserIdAsync(userId);
            if (spokenLanguages == null || !spokenLanguages.Any())
            {
                return NotFound(new { Message = $"No spoken languages found for UserId {userId}." });
            }
            return Ok(spokenLanguages);
        }
        // GET: api/SpokenLanguages/user/current
        [HttpGet("current-user")]
        public async Task<IActionResult> GetByCurrentUserId()
        {
            var userId = GetUserId(); // Giả sử bạn đã có phương thức này để lấy UserId từ token.

            var spokenLanguages = await _spokenLanguagesRepository.GetSpokenLanguagesByUserIdAsync(userId);
            if (spokenLanguages == null || !spokenLanguages.Any())
            {
                return NotFound(new { Message = $"No spoken languages found for UserId {userId}." });
            }
            return Ok(spokenLanguages);
        }
        // GET: api/SpokenLanguages/1/1
        [HttpGet("{languagesId}/{userId}")]
        public async Task<IActionResult> GetById(int languagesId, int userId)
        {
            var spokenLanguage = await _spokenLanguagesRepository.GetSpokenLanguageByIdAsync(languagesId, userId);
            if (spokenLanguage == null)
            {
                return NotFound(new { Message = $"SpokenLanguage with LanguagesId {languagesId} and UserId {userId} not found." });
            }
            return Ok(spokenLanguage);
        }

        // POST: api/SpokenLanguages
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpokenLanguages newSpokenLanguage)
        {
            if (newSpokenLanguage == null)
            {
                return BadRequest("SpokenLanguage is null.");
            }

            var createdSpokenLanguage = await _spokenLanguagesRepository.AddSpokenLanguageAsync(newSpokenLanguage);
            return CreatedAtAction(nameof(GetById), new { languagesId = createdSpokenLanguage.LanguagesId, userId = createdSpokenLanguage.UserId }, createdSpokenLanguage);
        }
        // POST: api/SpokenLanguages
        [HttpPost("add-by-current-user")]
        public async Task<IActionResult> CreatebyCurrrentUser([FromBody] SpokenLanguages newSpokenLanguage)
        {
            if (newSpokenLanguage == null)
            {
                return BadRequest("SpokenLanguage is null.");
            }
            // Lấy UserId từ token
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }
            // Lấy UserId hiện tại
            newSpokenLanguage.UserId = userId;
     
            var createdSpokenLanguage = await _spokenLanguagesRepository.AddSpokenLanguageAsync(newSpokenLanguage);
            return CreatedAtAction(nameof(GetById), new { languagesId = createdSpokenLanguage.LanguagesId, userId = createdSpokenLanguage.UserId }, createdSpokenLanguage);
        }
        // PUT: api/SpokenLanguages/1/1
        [HttpPut("{languagesId}/{userId}")]
        public async Task<IActionResult> Update(int languagesId, int userId, [FromBody] SpokenLanguages updatedSpokenLanguage)
        {
            if (languagesId != updatedSpokenLanguage.LanguagesId || userId != updatedSpokenLanguage.UserId)
            {
                return BadRequest("SpokenLanguage ID mismatch.");
            }

            await _spokenLanguagesRepository.UpdateSpokenLanguageAsync(updatedSpokenLanguage);
            return NoContent();
        }

        // DELETE: api/SpokenLanguages/1/1
        [HttpDelete("{languagesId}/{userId}")]
        public async Task<IActionResult> Delete(int languagesId, int userId)
        {
            var spokenLanguage = await _spokenLanguagesRepository.GetSpokenLanguageByIdAsync(languagesId, userId);
            if (spokenLanguage == null)
            {
                return NotFound(new { Message = $"SpokenLanguage with LanguagesId {languagesId} and UserId {userId} not found." });
            }

            await _spokenLanguagesRepository.DeleteSpokenLanguageAsync(languagesId, userId);
            return NoContent();
        }
        // DELETE: api/SpokenLanguages/user/1
        [HttpDelete("current-user/{languagesId}")]
        public async Task<IActionResult> DeleteForUser(int languagesId)
        {

            var userId = GetUserId(); // Giả sử bạn có phương thức GetUserId để lấy UserId từ token.
                                      // Lấy UserId từ token
            
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }
            // Lấy thông tin SpokenLanguage theo languagesId
            var spokenLanguage = await _spokenLanguagesRepository.GetSpokenLanguageByIdAsync(languagesId, userId);
            if (spokenLanguage == null)
            {
                return NotFound(new { Message = $"SpokenLanguage with LanguagesId {languagesId} not found for UserId {userId}." });
            }

            // Kiểm tra xem UserId có phải là người tạo SpokenLanguage hay không
            if (spokenLanguage.UserId != userId)
            {
                return Forbid("You are not authorized to delete this SpokenLanguage.");
            }

            await _spokenLanguagesRepository.DeleteSpokenLanguageAsync(languagesId, userId);
            return NoContent();
        }
    }


}
