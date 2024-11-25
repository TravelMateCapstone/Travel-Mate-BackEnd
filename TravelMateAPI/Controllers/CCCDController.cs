using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CCCDController : ControllerBase
    {
        private readonly ICCCDRepository _repository;

        public CCCDController(ICCCDRepository repository)
        {
            _repository = repository;
        }
        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }
        // GET: api/CCCD
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cccds = await _repository.GetAllAsync();
            return Ok(cccds);
        }

        // GET: api/CCCD/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cccd = await _repository.GetByIdAsync(id);
            if (cccd == null)
            {
                return NotFound(new { Message = $"CCCD with ID {id} not found." });
            }
            return Ok(cccd);
        }

        // POST: api/CCCD/update-imageFront
        [HttpPost("create-imageFront")]
        public async Task<IActionResult> CreateImageFront([FromBody] string imageFront)
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Kiểm tra nếu CCCD đã tồn tại cho UserId
            var existingCCCD = await _repository.GetByUserIdAsync(userId);
            if (existingCCCD != null)
            {
                return BadRequest(new { Message = "CCCD already exists for the current user." });
            }

            // Tạo mới CCCD
            var newCCCD = new CCCD
            {
                UserId = userId,
                imageFront = imageFront,
                id = "",
                name = "",
                dob = "",
                sex = "",
                nationality = "",
                home = "",
                address = "",
                doe = "",
                imageBack = "",
                features = "",
                issue_date = "",
                mrz = new List<string>(),
                issue_loc = ""
            };

            // Lưu CCCD vào database
            await _repository.AddAsync(newCCCD);

            return Ok(new
            {
                Success = true,
                Message = "CCCD with imageFront created successfully.",
                Data = newCCCD
            });
        }


        // PUT: api/CCCD/update-details
        [HttpPut("update-details-front")]
        public async Task<IActionResult> UpdateCCCDDetailFronts([FromBody] CCCD updatedCCCD)
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy CCCD của người dùng từ database
            var cccd = await _repository.GetByUserIdAsync(userId);
            if (cccd == null)
            {
                return NotFound(new { Message = "CCCD not found for the current user." });
            }

            // Cập nhật các trường cần thiết (cccd.id, cccd.name, ...)
            cccd.id = updatedCCCD.id ?? cccd.id;
            cccd.name = updatedCCCD.name ?? cccd.name;
            cccd.dob = updatedCCCD.dob ?? cccd.dob;
            cccd.sex = updatedCCCD.sex ?? cccd.sex;
            cccd.nationality = updatedCCCD.nationality ?? cccd.nationality;
            cccd.home = updatedCCCD.home ?? cccd.home;
            cccd.address = updatedCCCD.address ?? cccd.address;
            cccd.doe = updatedCCCD.doe ?? cccd.doe;

            // Lưu thay đổi vào database
            await _repository.UpdateAsync(cccd);

            return Ok(new
            {
                Success = true,
                Message = "CCCD details updated successfully.",
                Data = cccd
            });
        }


        // PUT: api/CCCD/update-imageBack
        [HttpPut("update-imageBack")]
        public async Task<IActionResult> UpdateCCCDImageBack([FromBody] CCCD updatedCCCD)
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy CCCD của người dùng từ database
            var cccd = await _repository.GetByUserIdAsync(userId);
            if (cccd == null)
            {
                return NotFound(new { Message = "CCCD not found for the current user." });
            }

            // Cập nhật chỉ trường imageBack (giữ nguyên các trường khác)
            cccd.imageBack = updatedCCCD.imageBack ?? cccd.imageBack;

            // Lưu thay đổi vào database
            await _repository.UpdateAsync(cccd);

            return Ok(new
            {
                Success = true,
                Message = "CCCD imageBack updated successfully.",
                Data = cccd
            });
        }

        // PUT: api/CCCD/update-details-back
        [HttpPut("update-details-back")]
        public async Task<IActionResult> UpdateCCCDDetailBacks([FromBody] CCCD updatedCCCD)
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy CCCD của người dùng từ database
            var cccd = await _repository.GetByUserIdAsync(userId);
            if (cccd == null)
            {
                return NotFound(new { Message = "CCCD not found for the current user." });
            }

            // Cập nhật các trường features, issue_date, mrz, issue_loc (giữ nguyên các trường khác)
            cccd.features = updatedCCCD.features ?? cccd.features;
            cccd.issue_date = updatedCCCD.issue_date ?? cccd.issue_date;
            cccd.mrz = updatedCCCD.mrz ?? cccd.mrz;
            cccd.issue_loc = updatedCCCD.issue_loc ?? cccd.issue_loc;

            // Lưu thay đổi vào database
            await _repository.UpdateAsync(cccd);

            return Ok(new
            {
                Success = true,
                Message = "CCCD features, issue_date, mrz, and issue_loc updated successfully.",
                Data = cccd
            });
        }
        // POST: api/CCCD
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CCCD cccd)
        {
            if (cccd == null)
            {
                return BadRequest("CCCD is null.");
            }

            await _repository.AddAsync(cccd);
            return Ok(new { Success = true, Message = "CCCD added successfully.", Data = cccd });
        }

        // PUT: api/CCCD/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CCCD cccd)
        {
            if (cccd == null)
            {
                return BadRequest("CCCD is null.");
            }

            var existingCCCD = await _repository.GetByIdAsync(id);
            if (existingCCCD == null)
            {
                return NotFound(new { Message = $"CCCD with ID {id} not found." });
            }

            // Update properties
            existingCCCD.imageFront = cccd.imageFront;
            existingCCCD.id = cccd.id;
            existingCCCD.name = cccd.name;
            existingCCCD.dob = cccd.dob;
            existingCCCD.sex = cccd.sex;
            existingCCCD.nationality = cccd.nationality;
            existingCCCD.home = cccd.home;
            existingCCCD.address = cccd.address;
            existingCCCD.doe = cccd.doe;
            existingCCCD.imageBack = cccd.imageBack;
            existingCCCD.features = cccd.features;
            existingCCCD.issue_date = cccd.issue_date;
            existingCCCD.mrz = cccd.mrz;
            existingCCCD.issue_loc = cccd.issue_loc;

            await _repository.UpdateAsync(existingCCCD);
            return Ok(new { Success = true, Message = "CCCD updated successfully.", Data = existingCCCD });
        }

        // DELETE: api/CCCD/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cccd = await _repository.GetByIdAsync(id);
            if (cccd == null)
            {
                return NotFound(new { Message = $"CCCD with ID {id} not found." });
            }

            await _repository.DeleteAsync(id);
            return Ok(new { Success = true, Message = "CCCD deleted successfully." });
        }

        // GET: api/CCCD/check-cccd-exists
        [HttpGet("check-cccd-exists")]
        public async Task<IActionResult> CheckCCCDExists()
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Kiểm tra nếu CCCD đã tồn tại cho UserId
            var cccd = await _repository.GetByUserIdAsync(userId);
            if (cccd != null)
            {
                return Ok(new { Success = true, Message = "CCCD exists.", Data = true });
            }
            else
            {
                return Ok(new { Success = true, Message = "CCCD does not exist.", Data = false });
            }
        }

    }
}
