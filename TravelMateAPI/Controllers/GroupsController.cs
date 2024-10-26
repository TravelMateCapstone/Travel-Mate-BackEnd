using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {

        private readonly IGroupRepository _groupRepository;
        private const int pageSize = 6;

        public GroupsController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
            //return 3;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();
            var groups = await _groupRepository.GetGroupsAsync();
            if (groups == null)
                return NotFound();

            var totalCount = groups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            groups = groups.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            foreach (var group in groups)
            {
                group.NumberOfParticipants = await _groupRepository.CountGroupParticipants(group.GroupId);
            }

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

        [AllowAnonymous]
        // GET: api/Groups/CreatedGroup/id
        [HttpGet("{groupId}")]
        public async Task<ActionResult<Group>> GetGroupByIdAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);

            group.NumberOfParticipants = await _groupRepository.CountGroupParticipants(group.GroupId);
            if (group == null)
                return NotFound();

            return Ok(group);
        }

        // GET: api/Groups/CreatedGroups
        [HttpGet("CreatedGroups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetCreatedGroups([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();

            var groups = await _groupRepository.GetCreatedGroupsAsync(userId);
            foreach (var group in groups)
            {
                group.NumberOfParticipants = await _groupRepository.CountGroupParticipants(group.GroupId);
            }

            if (groups == null)
                return NotFound();

            var totalCount = groups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            groups = groups.Skip((pageNumber - 1) * pageSize).Take(pageSize);

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

        // GET: api/Groups/CreatedGroup/id
        [HttpGet("CreatedGroups/{groupId}")]
        public async Task<ActionResult<Group>> GetCreatedGroupByIdAsync(int groupId)
        {
            var userId = GetUserId();

            var group = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);

            group.NumberOfParticipants = await _groupRepository.CountGroupParticipants(group.GroupId);

            if (group == null)
                return NotFound();

            return Ok(group);
        }

        // GET: api/Groups/Joined
        [HttpGet("JoinedGroups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetJoinedGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();

            var joinedGroups = await _groupRepository.GetJoinedGroupsAsync(userId);

            foreach (var group in joinedGroups)
                group.NumberOfParticipants = await _groupRepository.CountGroupParticipants(group.GroupId);


            if (joinedGroups == null)
                return NotFound();

            var totalCount = joinedGroups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            joinedGroups = joinedGroups.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var result = new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Groups = joinedGroups
            };

            return Ok(result);
        }

        // POST: api/Groups/Join/5
        [HttpGet("JoinedGroups/{groupId}")]
        public async Task<IActionResult> GetJoinedGroupByIdAsync(int groupId)
        {
            var userId = GetUserId();

            var joinedGroups = _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);

            //joinedGroups.NumberOfParticipants = await _groupRepository.CountGroupParticipants(group.GroupId);

            if (joinedGroups == null)
                return NotFound();

            return Ok(joinedGroups);
        }

        [HttpPost("JoinedGroups/Join/{groupId}")]
        public async Task<IActionResult> JoinGroup(int groupId)
        {
            var userId = GetUserId();

            var existingParticipant = await _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);

            if (existingParticipant != null) return BadRequest();

            await _groupRepository.JoinGroup(userId, groupId);

            return Ok("Request Join group sent");
        }

        [HttpPost("JoinedGroups/{groupId}/AcceptJoin")]
        public async Task<IActionResult> AcceptJoinGroup([FromQuery] int requesterId, int groupId)
        {
            var userId = GetUserId();
            var isGroupCreator = _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (isGroupCreator == null) return NotFound();

            await _groupRepository.AcceptJoinGroup(requesterId, groupId);

            return Ok("Accepted a join group request");
        }

        // DELETE: api/Groups/Leave/5
        [HttpDelete("LeaveGroup/{groupId}")]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var userId = GetUserId();
            //var group = _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);
            //if (group == null)
            //    return NotFound();

            await _groupRepository.LeaveGroup(userId, groupId);

            return Ok("Leave group successfully");
        }

        // POST: api/Group
        //get creator id and group information
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] Group newGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //get user id from session
            var userId = GetUserId();
            newGroup.CreatedById = userId;
            // add user id to object
            await _groupRepository.AddAsync(newGroup);

            //test to check data 
            return Ok(newGroup);
        }

        // PUT: api/Group/5
        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] Group updatedGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (groupId != updatedGroup.GroupId)
            {
                return BadRequest("Group ID mismatch");
            }
            var userId = GetUserId();
            var existingGroup = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (existingGroup == null)
            {
                return NotFound();
            }
            await _groupRepository.UpdateAsync(updatedGroup);
            return NoContent();
        }

        // DELETE: api/Group/5
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var userId = GetUserId();
            var existingGroup = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (existingGroup == null)
            {
                return NotFound();
            }
            await _groupRepository.DeleteAsync(groupId);
            return NoContent();
        }

    }

}
