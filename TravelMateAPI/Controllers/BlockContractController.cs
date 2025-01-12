using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelMateAPI.Services.CCCDValid;
using TravelMateAPI.Services.Contract;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly ICCCDService _cCCDService;

        public BlockContractController(IContractService contractService,ICCCDService cCCDService)
        {
            _contractService = contractService;
            _cCCDService = cCCDService;
        }

        [HttpPost("create-contract")]
        public async Task<IActionResult> CreateContract([FromBody] CreateContractRequest request)
        {
            try
            {
                // Kiểm tra chữ ký số của traveler
                var isTravelerSignatureValid = await _cCCDService.VerifyDigitalSignatureAsync(request.TravelerId, request.TravelerSignature);
                if (!isTravelerSignatureValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Chữ ký số của traveler không hợp lệ."
                    });
                }
                // Kiểm tra chữ ký số của traveler
                var isLocalSignatureValid = await _cCCDService.VerifyDigitalSignatureAsync(request.LocalId, request.LocalSignature);
                if (!isLocalSignatureValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Chữ ký số của local không hợp lệ."
                    });
                }
                var newContract = _contractService.CreateContract(
                    request.TravelerId,
                    request.LocalId,
                    request.TourId,
                    request.Location,
                    request.Details,
                    "Created", // Trạng thái mặc định khi tạo hợp đồng
                    request.TravelerSignature,
                    request.LocalSignature
                );

                //return Ok(newContract);
                return Ok(new
                {
                    Success = true,
                    Message = "Tạo hợp đồng thành công.",
                    Data = newContract
                });
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("create-contract-local-pass")]
        public async Task<IActionResult> CreateContractLocalPass([FromBody] CreateContractLocalPassRequest request)
        {
            try
            {
                // Kiểm tra chữ ký số của traveler
                var isTravelerSignatureValid = await _cCCDService.VerifyDigitalSignatureAsync(request.TravelerId, request.TravelerSignature);
                if (!isTravelerSignatureValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Chữ ký số của traveler không hợp lệ."
                    });
                }
                // Kiểm tra chữ ký số của traveler
                
                var newContract = _contractService.CreateContractPassLocal(
                    request.TravelerId,
                    request.LocalId,
                    request.TourId,
                    request.ScheduleId,
                    request.Location,
                    request.Details,
                    "Created", // Trạng thái mặc định khi tạo hợp đồng
                    request.TravelerSignature
                );

                //return Ok(newContract);
                return Ok(new
                {
                    Success = true,
                    Message = "Tạo hợp đồng thành công.",
                    Data = newContract
                });
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpGet("contracts-by-traveler/{travelerId}")]
        public async Task<IActionResult> GetContractsByTravelerAsync(int travelerId)
        {
            try
            {
                var contracts = await _contractService.GetContractsByTravelerAsync(travelerId);

                if (contracts == null || !contracts.Any())
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Không tìm thấy hợp đồng nào của bạn trên vai trò là Khách Du Lịch."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Lấy danh sách hợp đồng thành công.",
                    Data = contracts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Đã xảy ra lỗi: {ex.Message}"
                });
            }
        }
        [HttpGet("contracts-by-local/{localId}")]
        public async Task<IActionResult> GetContractsByLocalAsync(int localId)
        {
            try
            {
                var contracts = await _contractService.GetContractsByLocalAsync(localId);

                if (contracts == null || !contracts.Any())
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Không tìm thấy hợp đồng nào của bạn trên vai trò là Người địa phương."
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Lấy danh sách hợp đồng thành công.",
                    Data = contracts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Đã xảy ra lỗi: {ex.Message}"
                });
            }
        }

        [HttpPost("update-status-completed")]
        public async Task<IActionResult> UpdateStatusToCompleted(int travelerId, int localId, string tourId, string scheduleId)
        {
            try
            {
                await _contractService.UpdateStatusToCompleted(travelerId, localId, tourId, scheduleId);
                return Ok("Trạng thái hợp đồng đã được cập nhật thành 'Completed'.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-status-cancelled")]
        public async Task<IActionResult> UpdateStatusToCancelled(int travelerId, int localId, string tourId, string scheduleId)
        {
            try
            {
                await _contractService.UpdateStatusToCancelled(travelerId, localId, tourId, scheduleId);
                return Ok("Trạng thái hợp đồng đã được cập nhật thành 'Cancelled'.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-to-database")]
        public async Task<IActionResult> SaveContractToDatabase(int travelerId, int localId, string tourId, string scheduleId)
        {
            try
            {
                await _contractService.SaveContractToDatabase(travelerId, localId, tourId, scheduleId);
                return Ok("Hợp đồng đã được lưu thành công vào database.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("verify-contract")]
        public async Task<IActionResult> VerifyContractIntegrityAsync([FromQuery] int travelerId, [FromQuery] int localId, [FromQuery] string tourId, [FromQuery] string scheduleId)
        {
            try
            {
                var isValid = await _contractService.VerifyContractIntegrityAsync(travelerId, localId, tourId, scheduleId);
                return Ok(new { IsValid = isValid });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpGet("local-contracts-count")]
        public async Task<IActionResult> GetContractCountAsLocalAsync([FromQuery] int userId)
        {
            try
            {
                var count = await _contractService.GetContractCountAsLocalAsync(userId);
                return Ok(new { UserId = userId, ContractCount = count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpGet("location-contracts-count")]
        public async Task<IActionResult> GetContractCountLocationAsync([FromQuery] string location)
        {
            try
            {
                var count = await _contractService.GetContractLocationCountAsync(location);
                return Ok(new {Location = location, ContractCount = count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("check-contract-status")]
        public async Task<IActionResult> CheckContractStatusAsync(int travelerId, string tourId, string scheduleId)
        {
            try
            {
                var status = await _contractService.CheckContractStatusAsync(travelerId, tourId,scheduleId);
                return Ok(new
                {
                    Success = true,
                    Message = "Trạng thái hợp đồng được lấy thành công.",
                    Status = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpGet("get-Locations-History/{travelerId}")]
        public async Task<IActionResult> GetLocationsByTravelerIdAsync(int travelerId)
        {
            try
            {
                var locations = await _contractService.GetLocationsByTravelerIdAsync(travelerId);
                return Ok(new
                {
                    Success = true,
                    Message = "Danh sách địa điểm đã đi được lấy thành công.",
                    Data = locations
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("get-top-location-details/{top}")]
        public async Task<IActionResult> GetTopLocationDetailsAsync(int top)
        {
            try
            {
                var topLocations = await _contractService.GetTopLocationsDetailsAsync(top);
                return Ok(new
                {
                    Success = true,
                    Message = "Lấy danh sách chi tiết Top 8 địa điểm thành công.",
                    Data = topLocations
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


    }
}
