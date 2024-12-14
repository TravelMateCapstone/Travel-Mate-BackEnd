using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelMateAPI.MLModels;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationPredictController : ControllerBase
    {
        //private readonly ModelPredictor _modelPredictor;

        //public LocationPredictController()
        //{
        //    _modelPredictor = new ModelPredictor();
        //}



        //[HttpGet("train")]
        //public IActionResult Train()
        //{
        //    try
        //    {
        //        ModelTrainer.TrainModel();
        //        return Ok("Model training completed.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        //[HttpGet("predict")]
        //public IActionResult Predict([FromQuery] string query)
        //{
        //    try
        //    {
        //        var location = ModelPredictor.PredictLocation(query);
        //        return Ok(new { Query = query, PredictedLocation = location });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        private readonly ModelPredictor _locationPredictor;

        public LocationPredictController(ModelPredictor locationPredictor)
        {
            _locationPredictor = locationPredictor;
        }

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

        //[HttpGet("predict")]
        //public IActionResult PredictLocation([FromQuery] string query)
        //{
        //    try
        //    {
        //        // Dự đoán địa điểm từ query
        //        var predictedLocation = _locationPredictor.PredictLocation(query);

        //        // Lấy chi tiết địa điểm từ cơ sở dữ liệu
        //        var locationDetails = _locationPredictor.GetLocationDetails(predictedLocation);

        //        if (locationDetails != null)
        //        {
        //            return Ok(new
        //            {
        //                PredictedLocation = predictedLocation,
        //                LocationId = locationDetails.LocationId,
        //                LocationName = locationDetails.LocationName,
        //                Title = locationDetails.Title,
        //                Description = locationDetails.Description,
        //                Image = locationDetails.Image,
        //                MapHtml = locationDetails.MapHtml
        //            });
        //        }
        //        else
        //        {
        //            return NotFound(new { Message = "Location not found in database." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
        //    }
        //}

        [HttpGet("predict")]
        public IActionResult PredictLocation([FromQuery] string query)
        {
            try
            {
                // Dự đoán danh sách các địa điểm từ câu hỏi của người dùng
                var predictedLocations = _locationPredictor.PredictLocation(query);

                // Lấy thông tin chi tiết về các địa điểm dự đoán từ cơ sở dữ liệu
                var locationsDetails = _locationPredictor.GetLocationsDetails(predictedLocations);
                // Nếu không tìm thấy thông tin chi tiết, trả về danh sách các địa điểm dự đoán
                if (locationsDetails == null || !locationsDetails.Any())
                {
                    return Ok(predictedLocations);  // Trả về danh sách các địa điểm dự đoán
                }
                return Ok(locationsDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}
