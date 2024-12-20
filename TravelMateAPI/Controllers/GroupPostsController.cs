﻿using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Groups/{groupId}/GroupPosts")]
    public class GroupPostsController : Controller
    {
        private readonly IGroupPostRepository _groupPostRepository;
        private readonly IMapper _mapper;

        public GroupPostsController(IGroupPostRepository groupPostRepository, IMapper mapper)
        {
            _groupPostRepository = groupPostRepository;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPostDTO>>> GetGroupPostsAsync(int groupId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                //check if you are a member of admin of 
                var IsMemberOrAdmin = await _groupPostRepository.IsMemberOrAdmin(userId, groupId);
                if (!IsMemberOrAdmin)
                    return BadRequest("Access Denied, You are not member of group");


                var groupPosts = await _groupPostRepository.GetGroupPostsAsync(groupId);
                //if (groupPosts == null || !groupPosts.Any())
                //    return NotFound(new { Message = "No post found." });

                // Map the list of GroupPost to a list of GroupPostDTO
                var groupPostDTOs = _mapper.Map<IEnumerable<GroupPostDTO>>(groupPosts);

                return Ok(groupPostDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the group post list.", Details = ex.Message });
            }
        }


        [HttpGet("{postId}")]
        public async Task<ActionResult<GroupPostDTO>> GetGroupPostByIdAsync(int groupId, int postId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if you are a member of admin of 
            var IsMemberOrAdmin = await _groupPostRepository.IsMemberOrAdmin(userId, groupId);
            if (!IsMemberOrAdmin)
                return BadRequest("Access Denied, You are not member of group");

            var groupPost = await _groupPostRepository.GetGroupPostByIdAsync(postId);
            //if (groupPost == null || groupPost.GroupId != groupId)
            //    return NotFound(new { Message = "No post found." });

            var groupPostDTO = _mapper.Map<GroupPostDTO>(groupPost);

            return Ok(groupPostDTO);
        }

        [HttpPost]
        public async Task<ActionResult<GroupPost>> CreateGroupPost(int groupId, [FromBody] GroupPost newGroupPost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if you are a member of admin of 
            var IsMemberOrAdmin = await _groupPostRepository.IsMemberOrAdmin(userId, groupId);
            if (!IsMemberOrAdmin)
                return BadRequest("Access Denied, You are not member of group");

            newGroupPost.GroupId = groupId;
            newGroupPost.CreatedTime = GetTimeZone.GetVNTimeZoneNow();
            newGroupPost.PostById = userId;

            if (newGroupPost.GroupPostPhotos != null && newGroupPost.GroupPostPhotos.Any())
            {
                foreach (var photo in newGroupPost.GroupPostPhotos)
                {
                    photo.PostId = newGroupPost.GroupPostId;
                }
            }
            await _groupPostRepository.AddAsync(newGroupPost);
            return Ok(newGroupPost);
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdateGroupPost(int groupId, int postId, [FromBody] GroupPost updatedGroupPost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existPost = await _groupPostRepository.GetGroupPostByIdAsync(postId);
            if (existPost == null)
                return NotFound("Post does not exist");

            //check if you are a member 
            var IsMemberOrAdmin = await _groupPostRepository.IsMemberOrAdmin(userId, groupId);
            if (!IsMemberOrAdmin)
                return BadRequest("Access Denied, You are not member of group");

            //check if you are group post creator
            var IsGroupPostCreator = await _groupPostRepository.IsGroupPostCreator(postId, userId);
            if (!IsGroupPostCreator)
                return BadRequest("Access Denied, You are not post creator");

            //kiểm tra post có thuộc group ko
            var isPostExistInGroup = await _groupPostRepository.IsPostExistInGroup(groupId, postId);
            if (isPostExistInGroup == null)
                return NotFound("Post does not exist in the group");

            //cập nhật data mới cho các trường
            existPost.Title = updatedGroupPost.Title;

            if (existPost.GroupPostPhotos != null && existPost.GroupPostPhotos.Any())
            {
                foreach (var photo in existPost.GroupPostPhotos)
                {
                    existPost.GroupPostPhotos.Remove(photo);
                }
            }

            if (updatedGroupPost.GroupPostPhotos != null && updatedGroupPost.GroupPostPhotos.Any())
            {
                foreach (var photo in updatedGroupPost.GroupPostPhotos)
                {
                    existPost.GroupPostPhotos.Add(photo);
                }
            }

            await _groupPostRepository.UpdateAsync(existPost);
            return NoContent();
        }


        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeleteGroupPost(int groupId, int postId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existPost = await _groupPostRepository.GetGroupPostByIdAsync(postId);
            if (existPost == null || existPost.GroupId != groupId)
                return NotFound(new { Message = "Group not found or access denied." });

            //check if you are a member of admin of 
            var IsMemberOrAdmin = await _groupPostRepository.IsMemberOrAdmin(userId, groupId);
            if (!IsMemberOrAdmin)
                return BadRequest("Access Denied, You are not member of group");

            //check if you are post creator
            var IsGroupPostCreator = await _groupPostRepository.IsGroupPostCreator(postId, userId);
            if (!IsGroupPostCreator)
                return BadRequest("Access Denied, You are not post creator");

            await _groupPostRepository.DeleteAsync(postId);
            return NoContent();
        }
    }
}
