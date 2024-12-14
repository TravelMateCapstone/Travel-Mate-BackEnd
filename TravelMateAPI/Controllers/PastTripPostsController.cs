using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace TravelMateAPI.Controllers
{
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
    }
}
