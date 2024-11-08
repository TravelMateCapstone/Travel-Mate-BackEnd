using AutoMapper;
using BusinessObjects.Utils.Response;
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
        private readonly IMapper _mapper;
        private const int pageSize = 6;

        public GroupsController(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        private async Task<ActionResult<IEnumerable<GroupDTO>>> PaginateAndRespondAsync(IEnumerable<Group> groups, int pageNumber)
        {
            var totalCount = groups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedGroups = groups.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var groupDTOs = _mapper.Map<IEnumerable<GroupDTO>>(paginatedGroups);
            return Ok(new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Groups = groupDTOs
            });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var groups = await _groupRepository.GetGroupsAsync();
            if (groups == null || !groups.Any())
                return NotFound(new { Message = "No groups found." });

            return await PaginateAndRespondAsync(groups, pageNumber);
        }

        [HttpGet("UnjoinedGroups")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetUnjoinedGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var groups = await _groupRepository.GetUnjoinedGroupsAsync(userId);
            if (groups == null || !groups.Any())
                return NotFound(new { Message = "No groups found." });

            return await PaginateAndRespondAsync(groups, pageNumber);
        }

        [AllowAnonymous]
        [HttpGet("{groupId}")]
        public async Task<ActionResult<GroupDTO>> GetGroupByIdAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group == null)
                return NotFound(new { Message = "Group not found." });

            var groupDTOs = _mapper.Map<GroupDTO>(group);

            return Ok(groupDTOs);
        }

        [HttpGet("CreatedGroups")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetCreatedGroups([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var groups = await _groupRepository.GetCreatedGroupsAsync(userId);
            if (groups == null || !groups.Any())
                return NotFound(new { Message = "No created groups found." });

            return await PaginateAndRespondAsync(groups, pageNumber);
        }

        [HttpGet("ListJoinGroupRequest/{groupId}")]
        public async Task<ActionResult<IEnumerable<GroupMemberDTO>>> ListJoinGroupRequest(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var groups = await _groupRepository.GetGroupByIdAsync(groupId);
            if (groups == null)
                return NotFound(new { Message = "No created groups found." });

            //check if you are group creator
            var isGroupCreator = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (isGroupCreator == null)
                return NotFound(new { Message = "Group not found or access denied." });

            var listParticipants = await _groupRepository.ListJoinGroupRequests(groupId);
            if (listParticipants == null)
                return NotFound("No request found");

            var groupMemberDTOs = _mapper.Map<IEnumerable<GroupMemberDTO>>(listParticipants);

            return Ok(groupMemberDTOs);

        }

        [HttpGet("CreatedGroups/{groupId}")]
        public async Task<ActionResult<GroupDTO>> GetCreatedGroupByIdAsync(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var group = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (group == null)
                return NotFound(new { Message = "Group not found." });

            var groupDTOs = _mapper.Map<GroupDTO>(group);

            return Ok(groupDTOs);
        }

        [HttpGet("JoinedGroups")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetJoinedGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var joinedGroups = await _groupRepository.GetJoinedGroupsAsync(userId);
            if (joinedGroups == null || !joinedGroups.Any())
                return NotFound(new { Message = "No joined groups found." });

            return await PaginateAndRespondAsync(joinedGroups, pageNumber);
        }

        [HttpGet("JoinedGroups/{groupId}")]
        public async Task<ActionResult<GroupDTO>> GetJoinedGroupByIdAsync(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var joinedGroup = await _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);
            if (joinedGroup == null)
                return NotFound(new { Message = "Joined group not found." });

            var groupDTOs = _mapper.Map<GroupDTO>(joinedGroup);

            return Ok(groupDTOs);
        }

        [HttpGet("{groupId}/Members")]
        public async Task<ActionResult<IEnumerable<GroupMemberDTO>>> GetGroupMembers(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if you are group member
            var isMember = await _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);
            var isCreator = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (isMember == null && isCreator == null)
                return BadRequest(new { Message = "You are not member of this group." });

            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group == null)
                return NotFound(new { Message = "Group not found." });

            var listGroupMembers = await _groupRepository.GetGroupMembers(groupId);
            if (listGroupMembers == null)
                return NotFound("No members were found in the group.");

            var groupMemberDTOs = _mapper.Map<IEnumerable<GroupMemberDTO>>(listGroupMembers);

            return Ok(groupMemberDTOs);
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

            var isRequestSent = await _groupRepository.DoesRequestSend(groupId, userId);
            if (isRequestSent)
                return NotFound("You have sent join request!");

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
                return NotFound(new { Message = "Access denied. You are not a group admin" });

            //update new data for group participant
            var updateMember = await _groupRepository.GetJoinRequestParticipant(requesterId, groupId);
            if (updateMember != null)
            {
                updateMember.JoinedStatus = true;
                updateMember.JoinAt = DateTime.Now;
                updateMember.Group.NumberOfParticipants += 1;
            }

            await _groupRepository.AcceptJoinGroup(updateMember);

            return Ok("Join request accepted.");
        }

        [HttpPost("JoinedGroups/{groupId}/RejectJoinGroup")]
        public async Task<IActionResult> RejectJoinGroup([FromQuery] int requesterId, int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var isGroupCreator = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (isGroupCreator == null)
                return NotFound(new { Message = "Access denied. You are not a group admin" });

            var removeRequest = await _groupRepository.GetJoinRequestParticipant(requesterId, groupId);

            await _groupRepository.RejectJoinGroupRequest(removeRequest);

            return Ok("Reject request successful.");
        }


        [HttpDelete("LeaveGroup/{groupId}")]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if you are admin or member
            var isAdmin = await _groupRepository.GetCreatedGroupByIdAsync(userId, groupId);
            if (isAdmin != null)
                return BadRequest("Can not leave! You are admin of this group");

            var isMember = await _groupRepository.GetJoinedGroupByIdAsync(userId, groupId);
            if (isMember == null)
                return BadRequest(new { Message = "You are not a member of this group" });

            var getGroupParticipant = await _groupRepository.GetGroupMember(userId, groupId);
            getGroupParticipant.Group.NumberOfParticipants -= 1;
            await _groupRepository.LeaveGroup(getGroupParticipant);

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
            newGroup.CreateAt = DateTime.Now;

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
