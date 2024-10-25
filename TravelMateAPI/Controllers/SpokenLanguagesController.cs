using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

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

        // GET: api/SpokenLanguages
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var spokenLanguages = await _spokenLanguagesRepository.GetAllSpokenLanguagesAsync();
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
    }


}
