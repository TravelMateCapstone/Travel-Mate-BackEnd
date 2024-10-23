using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversityController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        // GET: api/University
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var universities = await _universityRepository.GetAllUniversitiesAsync();
            return Ok(universities);
        }

        // GET: api/University/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(id);
            if (university == null)
            {
                return NotFound(new { Message = $"University with id {id} not found." });
            }
            return Ok(university);
        }

        // POST: api/University
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] University newUniversity)
        {
            if (newUniversity == null)
            {
                return BadRequest("University is null.");
            }

            var createdUniversity = await _universityRepository.AddUniversityAsync(newUniversity);
            return CreatedAtAction(nameof(GetById), new { id = createdUniversity.UniversityId }, createdUniversity);
        }

        // PUT: api/University/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] University updatedUniversity)
        {
            if (id != updatedUniversity.UniversityId)
            {
                return BadRequest("University ID mismatch.");
            }

            await _universityRepository.UpdateUniversityAsync(updatedUniversity);
            return NoContent();
        }

        // DELETE: api/University/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(id);
            if (university == null)
            {
                return NotFound(new { Message = $"University with id {id} not found." });
            }

            await _universityRepository.DeleteUniversityAsync(id);
            return NoContent();
        }
    }

}
