using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using TravelMateAPI.Services.FilterTour;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterToursWOOController : ControllerBase
    {
        private readonly FilterTourService _tourService;

        public FilterToursWOOController(FilterTourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet("GetAllTour-WithUserDetails")]
        public async Task<IActionResult> GetAllTourBriefWithUserDetails()
        {
            try
            {
                var result = await _tourService.GetAllTourBriefWithUserDetailsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllTour-WithUserDetails-ByLocation")]
        public async Task<IActionResult> GetAllTourBriefWithUserDetailsByLocation(string location)
        {
            try
            {
                var result = await _tourService.GetAllTourBriefWithUserDetailsByLocationAsync(location);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
