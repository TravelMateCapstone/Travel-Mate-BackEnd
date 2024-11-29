using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using TravelMateAPI.Services.Notification;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        private const int pageSize = 6;

        public GroupsController(UserManager<ApplicationUser> userManager, IGroupRepository groupRepository, IMapper mapper, INotificationService notificationService)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _userManager = userManager;
            _notificationService = notificationService;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroupsAsync([FromQuery] int pageNumber = 1)
        {

            var groups = await _groupRepository.GetGroupsAsync();
            //if (groups == null || !groups.Any())
            //    return NotFound(new { Message = "No groups found." });

            return await PaginateAndRespondAsync(groups, pageNumber);
        }

        [HttpGet("UnjoinedGroups")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetUnjoinedGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var groups = await _groupRepository.GetUnjoinedGroupsAsync(user.Id);
            //if (groups == null || !groups.Any())
            //    return NotFound(new { Message = "No groups found." });

            var totalCount = groups.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedGroups = groups.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var listUnjoinedGroups = new List<object>();

            // Add join status for each group in the list
            foreach (var group in paginatedGroups)
            {
                // Check if the user has sent a join request to this group
                var joinedStatus = await _groupRepository.DoesRequestSend(group.GroupId, user.Id) ? "Pending" : "Unjoin";

                listUnjoinedGroups.Add(new
                {
                    UserJoinedStatus = joinedStatus,
                    Group = group
                });
            }

            return Ok(new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                listUnjoinedGroups
            }
                );
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<GroupDTO>> GetGroupByIdAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            //if (group == null)
            //    return NotFound(new { Message = "Group not found." });

            var groupDTOs = _mapper.Map<GroupDTO>(group);

            return Ok(groupDTOs);
        }

        [HttpGet("CreatedGroups")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetCreatedGroups([FromQuery] int pageNumber = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var groups = await _groupRepository.GetCreatedGroupsAsync(user.Id);
            //if (groups == null || !groups.Any())
            //    return NotFound(new { Message = "No created groups found." });

            return await PaginateAndRespondAsync(groups, pageNumber);
        }

        [HttpGet("ListJoinGroupRequest/{groupId}")]
        public async Task<ActionResult<IEnumerable<GroupMemberDTO>>> ListJoinGroupRequest(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var groups = await _groupRepository.GetGroupByIdAsync(groupId);
            if (groups == null)
                return NotFound(new { Message = "No created groups found." });

            //check if you are group creator
            var isGroupCreator = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            if (isGroupCreator == null)
                return NotFound(new { Message = "Group not found or access denied." });

            var listParticipants = await _groupRepository.ListJoinGroupRequests(groupId);
            //if (listParticipants == null)
            //    return NotFound("No request found");

            var groupMemberDTOs = _mapper.Map<IEnumerable<GroupMemberDTO>>(listParticipants);

            return Ok(groupMemberDTOs);
        }

        [HttpGet("CreatedGroups/{groupId}")]
        public async Task<ActionResult<GroupDTO>> GetCreatedGroupByIdAsync(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var group = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            //if (group == null)
            //    return NotFound(new { Message = "Group not found." });

            var groupDTOs = _mapper.Map<GroupDTO>(group);

            return Ok(groupDTOs);
        }

        [HttpGet("JoinedGroups")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetJoinedGroupsAsync([FromQuery] int pageNumber = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var joinedGroups = await _groupRepository.GetJoinedGroupsAsync(user.Id);
            //if (joinedGroups == null || !joinedGroups.Any())
            //    return NotFound(new { Message = "No joined groups found." });

            return await PaginateAndRespondAsync(joinedGroups, pageNumber);
        }

        [HttpGet("JoinedGroups/{groupId}")]
        public async Task<ActionResult<GroupDTO>> GetJoinedGroupByIdAsync(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existingGroup = await _groupRepository.GetGroupByIdAsync(groupId);
            if (existingGroup == null)
                return NotFound("Group does not exist");

            var JoinedStatus = "Unjoin";

            var DoesRequestSend = await _groupRepository.DoesRequestSend(groupId, user.Id);
            if (DoesRequestSend)
                JoinedStatus = "Pending";

            //get join group
            var joinedGroup = await _groupRepository.GetJoinedGroupByIdAsync(user.Id, groupId);
            if (joinedGroup != null)
                JoinedStatus = "Joined";

            var groupDTO = _mapper.Map<GroupDTO>(existingGroup);

            return Ok(new
            {
                UserJoinedStatus = JoinedStatus,
                Group = groupDTO
            }
            );
        }

        [HttpGet("{groupId}/Members")]
        public async Task<ActionResult<IEnumerable<GroupMemberDTO>>> GetGroupMembers(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //check if you are group member
            var isMember = await _groupRepository.GetJoinedGroupByIdAsync(user.Id, groupId);
            var isCreator = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            if (isMember == null && isCreator == null)
                return BadRequest(new { Message = "You are not member of this group." });

            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group == null)
                return NotFound(new { Message = "Group not found." });

            var listGroupMembers = await _groupRepository.GetGroupMembers(groupId);
            //if (listGroupMembers == null)
            //    return NotFound("No members were found in the group.");

            var groupMemberDTOs = _mapper.Map<IEnumerable<GroupMemberDTO>>(listGroupMembers);

            return Ok(groupMemberDTOs);
        }

        [HttpPost("JoinedGroups/Join/{groupId}")]
        public async Task<IActionResult> JoinGroup(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var isCreator = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            if (isCreator != null)
                return BadRequest(new { Message = "You are creator of the group" });

            var isRequestSent = await _groupRepository.DoesRequestSend(groupId, user.Id);
            if (isRequestSent)
                return NotFound("You have sent join request!");

            var isMember = await _groupRepository.GetJoinedGroupByIdAsync(user.Id, groupId);
            if (isMember != null)
                return BadRequest(new { Message = "You have already joined this group." });

            var newParticipant = new GroupParticipant
            {
                UserId = user.Id,
                GroupId = groupId,
                JoinedStatus = false,
                RequestAt = GetTimeZone.GetVNTimeZoneNow()
            };

            await _groupRepository.JoinGroup(newParticipant);

            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            await _notificationService.CreateNotificationFullAsync(group.CreatedById, $"{user.FullName} đã gửi yêu cầu vào nhóm {group.GroupName} của bạn.", group.GroupId, 4);
            return Ok("Join request sent.");
        }

        [HttpPost("JoinedGroups/{groupId}/AcceptJoin")]
        public async Task<IActionResult> AcceptJoinGroup([FromQuery] int requesterId, int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var isGroupCreator = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            if (isGroupCreator == null)
                return NotFound(new { Message = "Access denied. You are not a group admin" });

            //update new data for group participant
            var updateMember = await _groupRepository.GetJoinRequestParticipant(requesterId, groupId);
            if (updateMember != null)
            {
                updateMember.JoinedStatus = true;
                updateMember.JoinAt = GetTimeZone.GetVNTimeZoneNow();
                updateMember.Group.NumberOfParticipants += 1;
            }

            await _groupRepository.AcceptJoinGroup(updateMember);

            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            await _notificationService.CreateNotificationFullAsync(requesterId, $"{user.FullName} đã chấp nhận yêu cầu vào nhóm {group.GroupName}.", group.GroupId, 4);
            return Ok("Join request accepted.");
        }

        [HttpPost("JoinedGroups/{groupId}/RejectJoinGroup")]
        public async Task<IActionResult> RejectJoinGroup([FromQuery] int requesterId, int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var isGroupCreator = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            if (isGroupCreator == null)
                return NotFound(new { Message = "Access denied. You are not a group admin" });

            var removeRequest = await _groupRepository.GetJoinRequestParticipant(requesterId, groupId);

            await _groupRepository.RejectJoinGroupRequest(removeRequest);

            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            await _notificationService.CreateNotificationFullAsync(requesterId, $"{user.FullName} đã từ chối yêu cầu vào nhóm {group.GroupName}.", user.Id, 4);

            return Ok("Reject request successful.");
        }


        [HttpDelete("LeaveGroup/{groupId}")]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //check if you are admin or member
            var isAdmin = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            if (isAdmin != null)
                return BadRequest("Can not leave! You are admin of this group");

            var isMember = await _groupRepository.GetJoinedGroupByIdAsync(user.Id, groupId);
            if (isMember == null)
                return BadRequest(new { Message = "You are not a member of this group" });

            var getGroupParticipant = await _groupRepository.GetGroupMember(user.Id, groupId);
            getGroupParticipant.Group.NumberOfParticipants -= 1;
            await _groupRepository.LeaveGroup(getGroupParticipant);

            return Ok("Left group successfully.");
        }

        [HttpDelete("CancelJoinRequest/{groupId}")]
        public async Task<IActionResult> CancelJoinRequest(int groupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var DoesRequestSend = await _groupRepository.DoesRequestSend(groupId, user.Id);
            if (!DoesRequestSend)
                return NotFound("No request was sent!");

            var getGroupParticipant = await _groupRepository.GetJoinRequestParticipant(user.Id, groupId);

            await _groupRepository.LeaveGroup(getGroupParticipant);

            return Ok("Cancel join request successfully.");
        }


        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] Group newGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            newGroup.CreatedById = user.Id;
            newGroup.CreateAt = GetTimeZone.GetVNTimeZoneNow();

            await _groupRepository.AddAsync(newGroup);
            return Ok(newGroup);
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] Group updatedGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existingGroup = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existingGroup = await _groupRepository.GetCreatedGroupByIdAsync(user.Id, groupId);
            if (existingGroup == null)
                return NotFound(new { Message = "Group not found or access denied." });

            await _groupRepository.DeleteAsync(groupId);
            return NoContent();
        }

    }
}
