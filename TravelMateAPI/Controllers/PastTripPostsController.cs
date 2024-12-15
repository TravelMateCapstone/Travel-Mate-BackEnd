﻿using AutoMapper;
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllPosts(int userId)
        {
            var posts = await _pastTripPostRepository.GetAllPostOfUserAsync(userId);

            return Ok(posts);
        }

        [HttpPost()]
        public async Task<IActionResult> AddPost([FromBody] PastTripPostTravelerDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check tour có tồn tại ko
            var existingTour = await _tourRepository.GetTourById(postDto.TourId);
            if (existingTour == null)
            {
                return BadRequest("Tour does not exist!");
            }

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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existingPost = await _pastTripPostRepository.GetPostByIdAsync(postId);
            if (existingPost == null)
            {
                return NotFound("Post not found.");
            }
            //kieerm tra co phai la traveler ko
            if (existingPost.TravelerId != user.Id)
            {
                return BadRequest("Access Denied! You are not creator of this post!");
            }

            await _pastTripPostRepository.DeleteAsync(postId);
            return Ok();
        }
    }
}
