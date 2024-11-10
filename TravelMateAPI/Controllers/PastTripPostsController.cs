using AutoMapper;
using BusinessObjects.Entities;
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
                return NotFound(new { Message = "No groups found." });

            var PastTripPostDTOs = _mapper.Map<IEnumerable<PastTripPostDTO>>(posts);

            return Ok(PastTripPostDTOs);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PastTripPost>>> GetAllPostOfUserAsync()
        {
            //check author
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });
            //check post exist
            //check if traveler or local
            var posts = await _repository.GetAllAsync();
            return Ok(posts);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PastTripPost>> GetById(int id)
        {
            //check author
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });
            //check post exist
            //check if traveler or local
            var post = await _repository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PastTripPost post)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check if traveler and local
            var IsTraveler = _repository.IsTraveler(userId);
            if (IsTraveler == null)
                return NotFound("You are not traveler");

            await _repository.AddAsync(post);

            return Ok(post);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTravelerPartAsync(int id, PastTripPost post)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check post exist
            //check if traveler and local

            if (id != post.PastTripPostId)
            {
                return BadRequest();
            }
            //await _repository.UpdateAsync(post);
            return NoContent();
        }




        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLocalPartAsync(int id, PastTripPost post)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check post exist
            //check if traveler and local

            if (id != post.PastTripPostId)
            {
                return BadRequest();
            }
            //await _repository.UpdateAsync(post);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == -1)
                return Unauthorized(new { Message = "Unauthorized access." });

            //check post exist
            //check if traveler and local

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
