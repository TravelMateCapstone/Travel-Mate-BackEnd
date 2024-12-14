using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelMateAPI.Services.ReportUser;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReportController : ControllerBase
    {
        private readonly IUserReportService _userReportService;

        public UserReportController(IUserReportService userReportService)
        {
            _userReportService = userReportService;
        }
        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        // GET: api/UserReport
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _userReportService.GetAllReportsAsync();
            return Ok(reports);
        }

        // GET: api/UserReport/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var report = await _userReportService.GetReportByIdAsync(id);
            if (report == null)
                return NotFound(new { Message = "Report not found." });
            return Ok(report);
        }

        // POST: api/UserReport
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] UserReport report)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdReport = await _userReportService.CreateReportAsync(report);
            // Return thông báo thành công khi tạo mới
            return Ok(createdReport);
        }

        [Authorize]
        [HttpPost("Send-Report")]
        public async Task<IActionResult> SendReportToPlatform([FromBody] UserReport report)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Lấy UserId từ token của người dùng hiện tại
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Invalid token or user not authenticated." });

                // Gán UserId hiện tại vào report
                report.UserId = userId;
                report.CreatedAt = GetTimeZone.GetVNTimeZoneNow();

                //// Thiết lập thời gian tạo
                //report.CreatedAt = DateTime.Now;

                var createdReport = await _userReportService.CreateReportAsync(report);
                // Return thông báo thành công khi tạo mới
                return Ok(new
                {
                    Success = true,
                    Message = "Report created successfully!",
                    Data = createdReport
                });
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về chi tiết lỗi
                // Thêm mã log để ghi log lỗi vào console hoặc file nếu cần
                return StatusCode(500, new { Message = "An error occurred while saving the entity changes.", Error = ex.Message });
            }
        }


        // PUT: api/UserReport/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] UserReport updatedReport)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var report = await _userReportService.UpdateReportAsync(id, updatedReport);
            if (report == null)
                return NotFound(new { Message = "Report not found." });

            return Ok(report);
        }

        // DELETE: api/UserReport/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var isDeleted = await _userReportService.DeleteReportAsync(id);
            if (!isDeleted)
                return NotFound(new { Message = "Report not found." });

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateStatus/{id}")]
        public async Task<IActionResult> UpdateReportStatus(int id, [FromBody] string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                return BadRequest(new { Message = "Status cannot be empty." });

            var updatedReport = await _userReportService.UpdateReportStatusAsync(id, newStatus);
            if (updatedReport == null)
                return NotFound(new { Message = "Report not found." });

            return Ok(updatedReport);
        }


    }
}
