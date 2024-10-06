using AutoMapper;
using BussinessObjects.Entities;
using BussinessObjects.Utils.Reponse;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLocationsWOOController : Controller
    {
        private readonly IUserLocationsRepository _userLocationsRepository;
        private readonly IMapper _mapper;
        public UserLocationsWOOController(IUserLocationsRepository userLocationsRepository, IMapper mapper)
        {
            _userLocationsRepository = userLocationsRepository;
            _mapper = mapper;
        }

        // GET: api/UserLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLocation>>> GetUserLocations()
        {
            var userLocations = await _userLocationsRepository.GetAllUserLocationsAsync();
            var userLocationDTOs = _mapper.Map<List<UserLocationDTO>>(userLocations);
            return Ok(userLocationDTOs);
        }
    }
}
