using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;
using TravelMateAPI.Services.CCCDValid;
using System.Security.Cryptography;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CCCDController : ControllerBase
    {
        private readonly ICCCDRepository _repository;
        private readonly ICCCDService _cccdService;

        public CCCDController(ICCCDRepository repository, ICCCDService cccdService)
        {
            _repository = repository;
            _cccdService = cccdService;
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
        // GET: api/CCCD/by-user/{userId}
        [HttpGet("By-User/{userId}")]
        public async Task<IActionResult> GetCCCDByUserId(int userId)
        {
            // Lấy thông tin CCCD từ Repository dựa trên UserId
            var cccd = await _repository.GetByUserIdAsync(userId);

            if (cccd == null)
            {
                return NotFound(new { Message = $"CCCD not found for user with ID {userId}." });
            }

            return Ok(new
            {
                Success = true,
                Data = cccd
            });
        }

        // GET: api/CCCD/by-user/{userId}
        [HttpGet("Current-User")]
        public async Task<IActionResult> GetCCCDCurrentUserId()
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy thông tin CCCD từ Repository dựa trên UserId
            var cccd = await _repository.GetByUserIdAsync(userId);

            if (cccd == null)
            {
                return NotFound(new { Message = $"CCCD not found for user with ID {userId}." });
            }

            return Ok(new
            {
                Success = true,
                Data = cccd
            });
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
                issue_loc = "",
                PublicSignature = ""
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


            //// Kiểm tra nếu ID của CCCD đã tồn tại trong dữ liệu (ngoại trừ bản ghi hiện tại)
            //if (!string.IsNullOrEmpty(updatedCCCD.id))
            //{
            //    var existingCCCD = await _repository.GetByIdCCCDAsync(updatedCCCD.id);
            //    if (existingCCCD != null && existingCCCD.UserId != userId)
            //    {
            //        return Conflict(new
            //        {
            //            Success = false,
            //            Message = "The provided CCCD ID already exists for another user."
            //        });
            //    }
            //}

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


        // PUT: api/CCCD/update-details
        [HttpPut("update-details-front-exit")]
        public async Task<IActionResult> UpdateCCCDDetailFrontExits([FromBody] CCCD updatedCCCD)
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


            // Kiểm tra nếu ID của CCCD đã tồn tại trong dữ liệu (ngoại trừ bản ghi hiện tại)
            if (!string.IsNullOrEmpty(updatedCCCD.id))
            {
                var existingCCCD = await _repository.GetByIdCCCDAsync(updatedCCCD.id);
                if (existingCCCD != null && existingCCCD.UserId != userId)
                {
                    return Conflict(new
                    {
                        Success = false,
                        Message = "The provided CCCD ID already exists for another user."
                    });
                }
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

        // PUT: api/CCCD/update-imageBack
        [HttpPut("add-publicKey")]
        public async Task<IActionResult> UpdatePublicKey([FromBody] CCCD updatedCCCD)
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
                return NotFound(new { Message = "Hãy cập nhật CCCD trước khi tạo chữ ký số" });
            }

            // Kiểm tra nếu người dùng đã có PublicSignature
            //if (!string.IsNullOrEmpty(cccd.PublicSignature))
            //{
            //    return BadRequest(new { Message = "Chữ ký của bạn đã được tạo, không thể cập nhật lại." });
            //}

            //// Cập nhật chỉ trường imageBack (giữ nguyên các trường khác)
            //cccd.PublicSignature = updatedCCCD.PublicSignature ?? cccd.PublicSignature;

            //// Lưu thay đổi vào database
            //await _repository.UpdateAsync(cccd);

            //return Ok(new
            //{
            //    Success = true,
            //    Message = "Bạn đã tạo chữ ký số thành công.",
            //    Data = cccd
            //});
            if (string.IsNullOrEmpty(updatedCCCD.PublicSignature))
            {
                return BadRequest(new { Message = "Chữ ký số không được để trống." });
            }

            // Băm PublicSignature với khóa bí mật
            var hashedSignature = HashWithSecretKey(updatedCCCD.PublicSignature);

            // Cập nhật trường PublicSignature
            cccd.PublicSignature = hashedSignature;

            // Lưu thay đổi vào database
            await _repository.UpdateAsync(cccd);

            return Ok(new
            {
                Success = true,
                Message = "Bạn đã tạo chữ ký số thành công.",
                Data = new
                {
                    cccd.PublicSignature, // Trả về chữ ký đã băm
                }
            });
        }

        private string HashWithSecretKey(string data)
        {
            var secretKey = "DAcaumongmoidieutotdep8386"; // Lấy khóa bí mật từ file env
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Secret key is missing from configuration.");
            }

            using var hmac = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var hashedBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashedBytes);
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
        // API kiểm tra mặt sau
        [HttpGet("verify-cccd")]
        public async Task<IActionResult> VerifyCCCD()
        {

            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            var isVerified = await _cccdService.IsVerifiedAsync(userId);

            if (!isVerified)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Bạn chưa xác minh CCCD đầy đủ."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CCCD đã được xác minh."
            });
        }

        // API kiểm tra chữ ký số
        [HttpGet("verify-public-signature")]
        public async Task<IActionResult> VerifyPublicSignature()
        {

            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            var isVerified = await _cccdService.IsPublicSignatureVerifiedAsync(userId);

            if (!isVerified)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Bạn chưa xác minh chữ ký số."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Chữ ký số đã được xác minh."
            });
        }


        [HttpGet("verify-cccd-signature")]
        public async Task<IActionResult> VerifyAll()
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            var result = await _cccdService.VerifyAllAsync(userId);

            return Ok(new
            {
                Success = result.IsFullyVerified,
                VerificationDetails = result
            });
        }


        // POST: api/CCCD/verify-signature
        [HttpPost("verify-signature")]
        public async Task<IActionResult> VerifySignature([FromBody] VerifySignatureRequest request)
        {
            try
            {
                // Kiểm tra chữ ký số
                var isValid = await _cccdService.VerifyDigitalSignatureAsync(request.UserId, request.PublicKey);

                if (isValid)
                {
                    return Ok(new { Success = true, Message = "Chữ ký số khớp." });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = "Chữ ký số không khớp." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
