using BusinessObjects.Entities;
using BusinessObjects.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;
using TravelMateAPI.Services.FilterLocal;

namespace TravelMateAPI.Controllers
{
    //[Route("odata/[controller]")]
    //[ApiController]
    public class ApplicationUsersController : ODataController
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly FilterUserService _filterService;

        public ApplicationUsersController(IApplicationUserRepository userRepository, FilterUserService filterService)
        {
            _userRepository = userRepository;
            _filterService = filterService;
        }

        // GET: odata/ApplicationUsers
        //[HttpGet("/")]
        //[EnableQuery]
        //public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUserDTO> queryOptions)
        //{
        //    var users = await _userRepository.GetAllUsersAsync();
        //    return Ok(users);
        //}
       
        [EnableQuery]
        public async Task<IActionResult> Get(ODataQueryOptions<UserWithDetailsDTO> queryOptions)
        {
            var users = await _filterService.GetAllUsersWithDetailsAsync();
            return Ok(users);
        }


        // GET: odata/ApplicationUsers(5)
        //[HttpGet("/{key}")]
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var user = await _userRepository.GetUserByIdAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }



        // POST: odata/ApplicationUsers
        //[HttpPost("/")]
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] ApplicationUser user)
        {
            await _userRepository.AddUserAsync(user);
            return Created(user);
        }

        // PUT: odata/ApplicationUsers(5)
        //[HttpPut("{key}")]
        [EnableQuery]
        public IActionResult Put([FromODataUri] int key, [FromBody] ApplicationUser user)
        {
            if (key != user.Id)
            {
                return BadRequest();
            }

            _userRepository.UpdateUser(user);
            return Updated(user);
        }

        // DELETE: odata/ApplicationUsers(5)
        //[HttpDelete("/{key}")]
        [EnableQuery]
        public IActionResult Delete([FromODataUri] int key)
        {
            var user = _userRepository.GetUserByIdAsync(key).Result;
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(user);
            return NoContent();
        }
    }
}
