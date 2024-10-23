using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguagesRepository _languagesRepository;

        public LanguagesController(ILanguagesRepository languagesRepository)
        {
            _languagesRepository = languagesRepository;
        }

        // GET: api/Languages
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var languages = await _languagesRepository.GetAllLanguagesAsync();
            return Ok(languages);
        }

        // GET: api/Languages/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var language = await _languagesRepository.GetLanguagesByIdAsync(id);
            if (language == null)
            {
                return NotFound(new { Message = $"Languages with id {id} not found." });
            }
            return Ok(language);
        }

        // POST: api/Languages
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Languages newLanguages)
        {
            if (newLanguages == null)
            {
                return BadRequest("Languages is null.");
            }

            var createdLanguages = await _languagesRepository.AddLanguagesAsync(newLanguages);
            return CreatedAtAction(nameof(GetById), new { id = createdLanguages.LanguagesId }, createdLanguages);
        }

        // PUT: api/Languages/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Languages updatedLanguages)
        {
            if (id != updatedLanguages.LanguagesId)
            {
                return BadRequest("Languages ID mismatch.");
            }

            await _languagesRepository.UpdateLanguagesAsync(updatedLanguages);
            return NoContent();
        }

        // DELETE: api/Languages/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var language = await _languagesRepository.GetLanguagesByIdAsync(id);
            if (language == null)
            {
                return NotFound(new { Message = $"Languages with id {id} not found." });
            }

            await _languagesRepository.DeleteLanguagesAsync(id);
            return NoContent();
        }
    }

}
