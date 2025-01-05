using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using TravelMateAPI.Services.FilterTour;

namespace TravelMateAPI.Controllers
{

    public class FilterToursController : ODataController
    {
        private readonly FilterTourService _tourService;

        public FilterToursController(FilterTourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        [EnableQuery] // Kích hoạt OData query options (filter, select, orderby, expand, etc.)
        public async Task<IActionResult> GetAllTourBriefWithUserDetails(ODataQueryOptions<TourWithUserDetailsDTO> queryOptions)
        {
            try
            {
                //var result = await _tourService.GetAllTourBriefWithUserDetailsAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
