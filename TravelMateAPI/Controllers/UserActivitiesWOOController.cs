using AutoMapper;
using BussinessObjects.Entities;
using BussinessObjects.Utils.Reponse;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivitiesWOOController : ControllerBase
    {
        private readonly IUserActivitiesRepository _userActivitiesRepository;
        private readonly IMapper _mapper;
        public UserActivitiesWOOController(IUserActivitiesRepository userActivitiesRepository, IMapper mapper)
        {
            _userActivitiesRepository = userActivitiesRepository;
            _mapper = mapper;
        }

        // GET: api/UserActivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserActivity>>> GetUserActivities()
        {
            var userActivities = await _userActivitiesRepository.GetAllUserActivitiesAsync();
            var userActivityDTOs = _mapper.Map<List<UserActivityDTO>>(userActivities);
            return Ok(userActivityDTOs);
        }
    }
}
