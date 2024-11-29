using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using BusinessObjects.Utils.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PastTripPostsController : ControllerBase
    {
        private readonly IPastTripPostRepository _repository;
        private readonly IMapper _mapper;

        public PastTripPostsController(IPastTripPostRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PastTripPostDTO>>> GetAll()
        {
            //check author
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check post exist
            var posts = await _repository.GetAllAsync();
            if (posts == null || !posts.Any())
                return NotFound(new { Message = "No posts found." });

            var PastTripPostDTOs = _mapper.Map<IEnumerable<PastTripPostDTO>>(posts);

            return Ok(PastTripPostDTOs);
        }

        [HttpGet("UserTrips/{userId}")]
        public async Task<ActionResult<IEnumerable<PastTripPostDTO>>> GetAllPostOfUserAsync(int userId)
        {
            var currentUserId = GetUserId();
            if (currentUserId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check post exist
            var posts = await _repository.GetAllPostOfUserAsync(userId);
            if (posts == null || !posts.Any())
                return NotFound(new { Message = "No posts found." });

            var postDTOs = _mapper.Map<IEnumerable<PastTripPostDTO>>(posts);

            return Ok(postDTOs);
        }


        [HttpGet("{pastTripPostId}")]
        public async Task<ActionResult<PastTripPostDTO>> GetById(int pastTripPostId)
        {
            //check author
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var post = await _repository.GetByIdAsync(pastTripPostId);
            if (post == null)
                return NotFound("No post exist");

            var postDTO = _mapper.Map<PastTripPostDTO>(post);

            return Ok(postDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromQuery] int travelerId, [FromQuery] int localId, PastTripPostInputDTO postDTO)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            if (travelerId != userId)
                return BadRequest("You are not traveler of this trip");

            var post = _mapper.Map<PastTripPost>(postDTO);

            post.TravelerId = travelerId;
            post.LocalId = localId;
            post.IsCaptionEdit = false;
            post.IsReviewEdited = false;
            post.CreatedAt = GetTimeZone.GetVNTimeZoneNow();

            await _repository.AddAsync(post);
            return Ok(post);
        }

        [HttpPut("{pastTripPostId}/TravelerUpdate")]
        public async Task<ActionResult> UpdateTravelerPartAsync(int pastTripPostId, TravelerPastTripPostDTO postDTO)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existingPost = await _repository.GetByIdAsync(pastTripPostId);
            if (existingPost == null)
                return NotFound(new { Message = "No posts found." });


            if (existingPost.TravelerId != userId)
                return BadRequest("Access Denied! You are not traveler of this trip");

            var travelerUpdatePost = _mapper.Map<PastTripPost>(postDTO);

            existingPost.Location = travelerUpdatePost.Location;
            existingPost.IsPublic = travelerUpdatePost.IsPublic;
            existingPost.Caption = travelerUpdatePost.Caption;
            existingPost.Star = travelerUpdatePost.Star;
            existingPost.IsCaptionEdit = true;

            if (existingPost.PostPhotos != null && existingPost.PostPhotos.Any())
            {
                foreach (var photo in existingPost.PostPhotos)
                {
                    existingPost.PostPhotos.Remove(photo);
                }
            }

            if (travelerUpdatePost.PostPhotos != null && travelerUpdatePost.PostPhotos.Any())
            {
                foreach (var photo in travelerUpdatePost.PostPhotos)
                {
                    existingPost.PostPhotos.Add(photo);
                }
            }

            await _repository.UpdateTravelerPartAsync(existingPost);

            return NoContent();
        }


        [HttpPut("{pastTripPostId}/LocalUpdate")]
        public async Task<ActionResult> UpdateLocalPartAsync(int pastTripPostId, LocalPastTripPostDTO postDTO)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            var existingPost = await _repository.GetByIdAsync(pastTripPostId);
            if (existingPost == null)
                return NotFound(new { Message = "No posts found." });


            if (existingPost.LocalId != userId)
                return BadRequest("Access Denied! You are not local of this trip");

            var localUpdatePost = _mapper.Map<PastTripPost>(postDTO);

            existingPost.Review = localUpdatePost.Review;
            existingPost.IsReviewEdited = true;

            await _repository.UpdateLocalPartAsync(existingPost);

            return NoContent();
        }


        [HttpDelete("{pastTripPostId}")]
        public async Task<ActionResult> Delete(int pastTripPostId)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check post exist
            var existingPost = await _repository.GetByIdAsync(pastTripPostId);
            if (existingPost == null)
                return NotFound(new { Message = "No posts found." });

            //check if traveler and local
            if (existingPost.TravelerId != userId)
                return BadRequest("Access Denied! You are not traveler of this trip");

            await _repository.DeleteAsync(pastTripPostId);
            return NoContent();
        }
    }
}
