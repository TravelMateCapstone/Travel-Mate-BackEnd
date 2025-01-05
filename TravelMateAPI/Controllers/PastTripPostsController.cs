using AutoMapper;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using Repository.Interfaces;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PastTripPostController : ControllerBase
    {
        private readonly IPastTripPostRepository _pastTripPostRepository;
        private readonly ITourRepository _tourRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public PastTripPostController(ITourRepository tourRepository, IMapper mapper, IPastTripPostRepository pastTripPostRepository, UserManager<ApplicationUser> userManager)
        {
            _pastTripPostRepository = pastTripPostRepository;
            _userManager = userManager;
            _mapper = mapper;
            _tourRepository = tourRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts([FromQuery] int userId)
        {
            var posts = await _pastTripPostRepository.GetAllPostOfUserAsync(userId);

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(string postId)
        {
            var existingPost = await _pastTripPostRepository.GetPostByIdAsync(postId);
            if (existingPost == null)
            {
                return BadRequest("Post does not exist!");
            }
            var posts = await _pastTripPostRepository.GetPostByIdAsync(postId);

            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] PastTripPostTravelerDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTour = await _tourRepository.GetTourById(postDto.TourId);
            if (existingTour == null)
            {
                return NotFound("Tour does not exist!");
            }

            if (existingTour.Creator.Id == postDto.TravelerId)
            {
                return BadRequest("Access Denied! You are the creator of this tour!");
            }

            ////check chỉ add dc 1 lần
            //var isParticipant = existingTour.Participants
            //                                .Any(p => p.ParticipantId == postDto.TravelerId);
            //if (!isParticipant)
            //{
            //    return BadRequest("You did not join this tour!");
            //}

            //var isPostCreated = existingTour.Participants.Any(p => p.ParticipantId == postDto.TravelerId && p.PostId != "");
            //if (isPostCreated)
            //{
            //    return BadRequest("You have already create post about this tour");
            //}
            var post = _mapper.Map<PastTripPost>(postDto);
            await _pastTripPostRepository.AddAsync(post);

            return Ok();
        }


        //CHƯA SỬA
        [HttpPut("traveler")]
        public async Task<IActionResult> UpdateTravelerPost([FromQuery] string postId, [FromBody] PastTripPostTravelerDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPost = await _pastTripPostRepository.GetPostByIdAsync(postId);
            if (existingPost == null)
            {
                return NotFound("Post not found.");
            }
            if (existingPost.TravelerId != postDto.TravelerId)
            {
                return BadRequest("Access Denied! You are not creator of this post!");
            }
            existingPost.IsCaptionEdit = true;
            existingPost.Caption = postDto.Caption;
            existingPost.Star = postDto.Star;
            existingPost.TripImages = postDto.TripImages;

            await _pastTripPostRepository.UpdatePostAsync(postId, existingPost);
            return Ok();
        }

        //CHƯA SỬA
        [HttpPut("local")]
        public async Task<IActionResult> UpdateLocalPost([FromQuery] string postId, [FromBody] PastTripPostLocalDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPost = await _pastTripPostRepository.GetPostByIdAsync(postId);
            if (existingPost == null)
            {
                return NotFound("Post not found.");
            }
            if (existingPost.LocalId != postDto.LocalId)
            {
                return BadRequest("Access Denied! You are not local of this post!");
            }

            if (existingPost.IsCommentEdited == null)
            {
                existingPost.IsCommentEdited = false;
            }
            else existingPost.IsCommentEdited = true;

            existingPost.Comment = postDto.Comment;

            await _pastTripPostRepository.UpdatePostAsync(postId, existingPost);
            return Ok();
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            var existingPost = await _pastTripPostRepository.GetPostByIdAsync(postId);
            if (existingPost == null)
            {
                return NotFound("Post not found.");
            }

            var getTour = await _tourRepository.GetTourById(existingPost.TourId);
            //foreach (var item in getTour.Participants)
            //{
            //    if (item.PostId == postId)
            //    {
            //        item.PostId = "";
            //        await _tourRepository.UpdateTour(getTour.TourId, getTour);
            //        break;
            //    }
            //}

            await _pastTripPostRepository.DeleteAsync(postId);
            return Ok();
        }
    }
}
