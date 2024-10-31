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
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var groups = await _groupRepository.GetGroupsAsync();
            if (groups == null || !groups.Any())
                return NotFound(new { Message = "No groups found." });

            var totalCount = groups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            groups = groups.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return Ok(new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Groups = groups
            });
        }

        [AllowAnonymous]
        [HttpGet("{groupId}")]
        public async Task<ActionResult<Group>> GetGroupByIdAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group == null)
                return NotFound(new { Message = "Group not found." });

            return Ok(group);
        }

        [HttpGet("CreatedGroups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetCreatedGroups([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var groups = await _groupRepository.GetCreatedGroupsAsync(userId);
            if (groups == null || !groups.Any())
                return NotFound(new { Message = "No created groups found." });

            var totalCount = groups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            groups = groups.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return Ok(new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Groups = groups
            });
        }

        [HttpGet("ListJoinGroupRequest/{groupId}")]
        public async Task<ActionResult<IEnumerable<GroupParticipant>>> ListJoinGroupRequest(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var groups = await _groupRepository.GetGroupByIdAsync(groupId);
            if (groups == null)
                return NotFound(new { Message = "No created groups found." });

            var listParticipants = await _groupRepository.ListJoinGroupRequests(groupId);

            return Ok(listParticipants);

        }

        [HttpGet("CreatedGroups/{groupId}")]
        public async Task<ActionResult<Group>> GetCreatedGroupByIdAsync(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var group = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (group == null)
                return NotFound(new { Message = "Group not found." });

            return Ok(group);
        }

        [HttpGet("JoinedGroups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetJoinedGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var joinedGroups = await _groupRepository.GetJoinedGroupsAsync(userId);
            if (joinedGroups == null || !joinedGroups.Any())
                return NotFound(new { Message = "No joined groups found." });

            var totalCount = joinedGroups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            joinedGroups = joinedGroups.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return Ok(new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Groups = joinedGroups
            });
        }

        [HttpGet("JoinedGroups/{groupId}")]
        public async Task<IActionResult> GetJoinedGroupByIdAsync(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var joinedGroup = await _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);
            if (joinedGroup == null)
                return NotFound(new { Message = "Joined group not found." });

            return Ok(joinedGroup);
        }

        [HttpPost("JoinedGroups/Join/{groupId}")]
        public async Task<IActionResult> JoinGroup(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });
            var isCreator = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (isCreator != null)
                return BadRequest(new { Message = "You are creator of the group" });

            var isMember = await _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);
            if (isMember != null)
                return BadRequest(new { Message = "You have already joined this group." });

            await _groupRepository.JoinGroup(userId, groupId);
            return Ok("Join request sent.");
        }

        [HttpPost("JoinedGroups/{groupId}/AcceptJoin")]
        public async Task<IActionResult> AcceptJoinGroup([FromQuery] int requesterId, int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var isGroupCreator = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (isGroupCreator == null)
                return NotFound(new { Message = "Group not found or access denied." });

            await _groupRepository.AcceptJoinGroup(requesterId, groupId);
            return Ok("Join request accepted.");
        }


        [HttpDelete("LeaveGroup/{groupId}")]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            await _groupRepository.LeaveGroup(userId, groupId);
            return Ok("Left group successfully.");
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] Group newGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            newGroup.CreatedById = userId;
            await _groupRepository.AddAsync(newGroup);
            return Ok(newGroup);
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] Group updatedGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existingGroup = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (existingGroup == null)
                return NotFound(new { Message = "Group not found or access denied." });

            existingGroup.GroupName = updatedGroup.GroupName;
            existingGroup.Description = updatedGroup.Description;
            existingGroup.Location = updatedGroup.Location;
            existingGroup.GroupImageUrl = updatedGroup.GroupImageUrl;

            await _groupRepository.UpdateAsync(existingGroup);
            return NoContent();
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existingGroup = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (existingGroup == null)
                return NotFound(new { Message = "Group not found or access denied." });

            await _groupRepository.DeleteAsync(groupId);
            return NoContent();
        }


    }
}
