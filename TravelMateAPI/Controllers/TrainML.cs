using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelMateAPI.MLModels;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainML : ControllerBase
    {
        [HttpGet("train")]
        public IActionResult Train()
        {
            try
            {
                ModelTrainer.TrainModel();
                return Ok("Model training completed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
