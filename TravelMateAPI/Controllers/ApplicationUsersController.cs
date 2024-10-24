using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    //[Route("odata/[controller]")]
    //[ApiController]
    public class ApplicationUsersController : ODataController
    {
        private readonly IApplicationUserRepository _userRepository;

        public ApplicationUsersController(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: odata/ApplicationUsers
        //[HttpGet("/")]
        [EnableQuery]
        public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUser> queryOptions)
        {
            var users = await _userRepository.GetAllUsersAsync();
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
