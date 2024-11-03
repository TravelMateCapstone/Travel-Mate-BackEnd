using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Groups/{groupId}/GroupPosts/{postId}/PostComments")]
    public class PostCommentsController : ControllerBase
    {
        private readonly IPostCommentRepository _postCommentRepository;

        public PostCommentsController(IPostCommentRepository postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        // GET: api/Groups/{groupId}/GroupPosts/{postId}/comments
        [HttpGet]
        public async Task<IActionResult> GetCommentsByPostId(int groupId, int postId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var isGroupMember = await _postCommentRepository.IsGroupMember(groupId, userId);
            if (!isGroupMember) return NotFound("Access Denied");

            var comments = await _postCommentRepository.GetAllAsync(postId);
            return Ok(comments);
        }

        // GET: api/Groups/{groupId}/GroupPosts/{postId}/comments/{commentId}
        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById(int groupId, int commentId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });
            var isGroupMember = await _postCommentRepository.IsGroupMember(groupId, userId);
            if (!isGroupMember) return NotFound("Access Denied");
            var comment = await _postCommentRepository.GetByIdAsync(commentId);
            if (comment == null)
                return NotFound("Comment not found");

            return Ok(comment);
        }

        // POST: api/Groups/{groupId}/GroupPosts/{postId}/comments
        [HttpPost]
        public async Task<IActionResult> AddComment(int groupId, int postId, [FromBody] PostComment newComment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if user is a group member
            var isGroupMember = await _postCommentRepository.IsGroupMember(groupId, userId);
            if (!isGroupMember) return NotFound("Access Denied");

            // Set group and post identifiers for the new comment
            newComment.PostId = postId;
            newComment.CommentedById = userId;
            newComment.CommentTime = DateTime.Now;
            var createdComment = await _postCommentRepository.AddAsync(newComment);
            return Ok(createdComment);
        }

        // PUT: api/Groups/{groupId}/GroupPosts/{postId}/comments/{commentId}
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] PostComment updatedComment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existingComment = await _postCommentRepository.GetByIdAsync(commentId);
            if (existingComment == null)
                return NotFound("Comment not found");

            //check if user is comment creator
            var isCommentCreator = await _postCommentRepository.IsCommentCreator(commentId, userId);
            if (!isCommentCreator)
                return NotFound("Access Denied!");

            // Update fields
            existingComment.CommentText = updatedComment.CommentText;
            existingComment.IsEdited = true;
            //existingComment.CommentTime = updatedComment.CommentTime;

            await _postCommentRepository.UpdateAsync(existingComment);
            return NoContent();
        }

        // DELETE: api/Groups/{groupId}/GroupPosts/{postId}/comments/{commentId}
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existingComment = await _postCommentRepository.GetByIdAsync(commentId);
            if (existingComment == null)
                return NotFound("Comment not found");

            //check if user is comment creator or groupadmin
            var isCommentor = await _postCommentRepository.IsCommentCreator(commentId, userId);
            if (!isCommentor)
                return NotFound("Access Denied");

            await _postCommentRepository.DeleteAsync(commentId);
            return NoContent();
        }
    }
}
