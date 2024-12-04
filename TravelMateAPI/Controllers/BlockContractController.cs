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


        [HttpPost("update-status-completed")]
        public async Task<IActionResult> UpdateStatusToCompleted(int travelerId, int localId, string tourId)
        {
            try
            {
                await _contractService.UpdateStatusToCompleted(travelerId, localId, tourId);
                return Ok("Trạng thái hợp đồng đã được cập nhật thành 'Completed'.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-status-cancelled")]
        public async Task<IActionResult> UpdateStatusToCancelled(int travelerId, int localId, string tourId)
        {
            try
            {
                await _contractService.UpdateStatusToCancelled(travelerId, localId, tourId);
                return Ok("Trạng thái hợp đồng đã được cập nhật thành 'Cancelled'.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-to-database")]
        public async Task<IActionResult> SaveContractToDatabase(int travelerId, int localId, string tourId)
        {
            try
            {
                await _contractService.SaveContractToDatabase(travelerId, localId, tourId);
                return Ok("Hợp đồng đã được lưu thành công vào database.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("verify-contract")]
        public async Task<IActionResult> VerifyContractIntegrityAsync([FromQuery] int travelerId, [FromQuery] int localId, [FromQuery] string tourId)
        {
            try
            {
                var isValid = await _contractService.VerifyContractIntegrityAsync(travelerId, localId, tourId);
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

    }
}
