using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {

        private readonly IGroupRepository _groupRepository;

        public GroupsController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups([FromQuery] int pageNumber = 1)
        {
            int pageSize = 9;
            var groups = await _groupRepository.GetAll();
            if (groups == null)
                return NotFound();
            var totalCount = groups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            groups = groups.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var result = new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Groups = groups
            };

            return Ok(result);
        }

        // GET: api/Group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroupById(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
                return NotFound();

            return Ok(group);
        }

        // POST: api/Group
        //get creator id and group information
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] Group newGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //get user id from session
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // add user id to object
            await _groupRepository.AddAsync(newGroup);
            //test to check data 
            return CreatedAtAction(nameof(GetGroupById), new { id = newGroup.GroupId }, newGroup);

            //return Created();

        }

        // PUT: api/Group/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] Group updatedGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id != updatedGroup.GroupId)
            {
                return BadRequest("Group ID mismatch");
            }

            var existingGroup = await _groupRepository.GetByIdAsync(id);
            if (existingGroup == null)
            {
                return NotFound();
            }

            await _groupRepository.UpdateAsync(updatedGroup);
            return NoContent();
        }

        // DELETE: api/Group/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            await _groupRepository.DeleteAsync(id);
            //return NoContent();
            //return NoContent();
            return NoContent();
        }

    }

}
