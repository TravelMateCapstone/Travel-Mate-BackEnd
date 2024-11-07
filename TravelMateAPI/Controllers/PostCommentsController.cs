using AutoMapper;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Response;
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
        private readonly IMapper _mapper;

        public PostCommentsController(IPostCommentRepository postCommentRepository, IMapper mapper)
        {
            _postCommentRepository = postCommentRepository;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        // GET: api/Groups/{groupId}/GroupPosts/{postId}/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostCommentDTO>>> GetCommentsByPostId(int groupId, int postId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if group member or admin
            var isGroupMemberOrAdmin = await _postCommentRepository.IsGroupMemberOrAdmin(groupId, userId);
            if (!isGroupMemberOrAdmin) return NotFound("Access Denied");

            var comments = await _postCommentRepository.GetAllAsync(postId);

            var commentDTOs = _mapper.Map<IEnumerable<PostCommentDTO>>(comments);
            return Ok(commentDTOs);
        }

        // GET: api/Groups/{groupId}/GroupPosts/{postId}/comments/{commentId}
        [HttpGet("{commentId}")]
        public async Task<ActionResult<PostCommentDTO>> GetCommentById(int groupId, int commentId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if group member or admin
            var isGroupMemberOrAdmin = await _postCommentRepository.IsGroupMemberOrAdmin(groupId, userId);
            if (!isGroupMemberOrAdmin) return NotFound("Access Denied");

            var comment = await _postCommentRepository.GetByIdAsync(commentId);
            if (comment == null)
                return NotFound("Comment not found");

            var commentDTO = _mapper.Map<PostCommentDTO>(comment);
            return Ok(commentDTO);
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

            //check if user is a group member or admin
            var isGroupMemberOrAdmin = await _postCommentRepository.IsGroupMemberOrAdmin(groupId, userId);
            if (!isGroupMemberOrAdmin) return NotFound("Access Denied");

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

            //check if user is comment creator or group admin
            var isCommentor = await _postCommentRepository.IsCommentCreator(commentId, userId);
            if (!isCommentor)
                return NotFound("Access Denied");

            await _postCommentRepository.DeleteAsync(commentId);
            return NoContent();
        }
    }
}
