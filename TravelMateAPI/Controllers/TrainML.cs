using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelMateAPI.MLModels;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainML : ControllerBase
    {

        private readonly ModelTrainer _modelTrainer;

        public TrainML(ModelTrainer modelTrainer)
        {
            _modelTrainer   = modelTrainer;
        }
        [HttpGet("train")]
        public IActionResult Train()
        {
            try
            {
                _modelTrainer.TrainModel();
                return Ok("Model training completed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
