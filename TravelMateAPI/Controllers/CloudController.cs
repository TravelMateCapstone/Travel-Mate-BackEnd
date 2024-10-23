using Microsoft.AspNetCore.Mvc;
using TravelMateAPI.Services.Storage;


namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudController : ControllerBase
    {
        private readonly ICloudStorageService _cloudStorageService;

        public CloudController(ICloudStorageService cloudStorageService)
        {
            _cloudStorageService = cloudStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string userId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ");
            }

            try
            {
                // Upload file lên Blob Storage
                string blobName = await _cloudStorageService.UploadFileAsync(userId, file.OpenReadStream(), file.FileName, file.ContentType);
                return Ok(new { Message = "File đã được tải lên thành công!", FileName = blobName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi xảy ra khi upload file: {ex.Message}");
            }
        }

        // Endpoint get file URL
        [HttpGet("get")]
        public async Task<IActionResult> GetFileUrl([FromQuery] string userId, [FromQuery] string fileName)
        {
            try
            {
                string fileUrl = await _cloudStorageService.GetFileUrlAsync(userId, fileName);
                return Ok(new { FileUrl = fileUrl });
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi xảy ra khi lấy file: {ex.Message}");
            }
        }

        // Endpoint get all files for a user
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllFiles([FromQuery] string userId)
        {
            try
            {
                List<string> fileUrls = await _cloudStorageService.GetAllFilesAsync(userId);
                return Ok(new { FileUrls = fileUrls });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi xảy ra khi lấy danh sách file: {ex.Message}");
            }
        }

    }
}
