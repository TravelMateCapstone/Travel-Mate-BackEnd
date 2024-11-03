using BusinessObjects.Entities;
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

        public GroupPostsController(IGroupPostRepository groupPostRepository)
        {
            _groupPostRepository = groupPostRepository;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupPost>>> GetGroupPostsAsync(int groupId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });
            var groupPosts = await _groupPostRepository.GetGroupPostsAsync(groupId);
            if (groupPosts == null || !groupPosts.Any())
                return NotFound(new { Message = "No post found." });

            return Ok(groupPosts);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<GroupPost>> GetGroupPostByIdAsync(int groupId, int postId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var groupPost = await _groupPostRepository.GetGroupPostByIdAsync(postId);
            if (groupPost == null || groupPost.GroupId != groupId)
                return NotFound(new { Message = "No post found." });

            return Ok(groupPost);
        }

        [HttpPost]
        public async Task<ActionResult<GroupPost>> CreateGroupPost(int groupId, [FromBody] GroupPost newGroupPost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            newGroupPost.GroupId = groupId;
            newGroupPost.CreatedTime = DateTime.UtcNow;
            newGroupPost.PostById = userId;

            if (newGroupPost.PostPhotos != null && newGroupPost.PostPhotos.Any())
            {
                foreach (var photo in newGroupPost.PostPhotos)
                {
                    photo.PostId = newGroupPost.PostId;
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

            //kiểm tra post có thuộc group ko
            var isPostExistInGroup = await _groupPostRepository.IsPostExistInGroup(groupId, postId);
            if (isPostExistInGroup == null)
                return NotFound("Post does not exist in the group");
            //cập nhật data mới cho các trường
            existPost.Title = updatedGroupPost.Title;

            if (existPost.PostPhotos != null && existPost.PostPhotos.Any())
            {
                foreach (var photo in existPost.PostPhotos)
                {
                    existPost.PostPhotos.Remove(photo);
                }
            }

            if (updatedGroupPost.PostPhotos != null && updatedGroupPost.PostPhotos.Any())
            {
                foreach (var photo in updatedGroupPost.PostPhotos)
                {
                    existPost.PostPhotos.Add(photo);
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

            await _groupPostRepository.DeleteAsync(postId);
            return NoContent();
        }
    }
}
