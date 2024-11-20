using AutoMapper;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using System.Security.Claims;
using TravelMateAPI.Services;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExtraFormDetailsController : ControllerBase
    {
        private readonly ILocalExtraDetailFormRepository _localRepository;
        private readonly ITravelerFormRepository _travelerRepository;
        private readonly IMapper _mapper;

        public ExtraFormDetailsController(ILocalExtraDetailFormRepository localExtraDetailFormRepository, ITravelerFormRepository travelerRepository, IMapper mapper)
        {
            _localRepository = localExtraDetailFormRepository;
            _travelerRepository = travelerRepository;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        [HttpGet("LocalForm")]
        public async Task<IActionResult> GetFormByUserId()
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                var form = await _localRepository.GetByUserIdAsync(userId);
                return Ok(form);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while retrieving the form.", Details = ex.Message });
            }
        }

        // PUT: api/ExtraFormDetails/LocalForm
        [HttpPut("LocalForm")]
        public async Task<IActionResult> UpdateForm([FromBody] LocalExtraDetailForm updatedForm)
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingForm = await _localRepository.GetByUserIdAsync(userId);

                if (existingForm == null)
                {
                    updatedForm.CreateById = userId;
                    updatedForm.CreatedAt = GetTimeZone.GetVNTimeZoneNow();
                    await _localRepository.CreateAsync(updatedForm);
                    return Ok("First time created successfully!");
                }

                existingForm.Services = updatedForm.Services;
                existingForm.Questions = updatedForm.Questions;
                existingForm.LatestUpdate = GetTimeZone.GetVNTimeZoneNow();

                await _localRepository.UpdateAsync(existingForm.FormId, existingForm);
                return Ok("Update successfully");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while updating the form.", Details = ex.Message });
            }
        }

        //TRAVELER

        // GET: api/TravelerForm
        [HttpGet("TravelerForm")]
        public async Task<IActionResult> GetFormRequest([FromQuery] int localId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                if (userId == localId)
                    return BadRequest("You can not request yourself!");

                var existingFormRequest = await _travelerRepository.GetByIdAsync(localId, userId);
                if (existingFormRequest != null)
                    return Ok(existingFormRequest);

                var localForm = await _localRepository.GetByUserIdAsync(localId);
                var formRequest = _mapper.Map<TravelerExtraDetailForm>(localForm);

                return Ok(formRequest);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while processing the form request.", Details = ex.Message });
            }
        }

        // PUT: api/TravelerForm/{FormId}
        [HttpPut("TravelerForm/{FormId}")]
        public async Task<IActionResult> UpdateTravelerForm(string FormId, [FromQuery] int localId, [FromBody] TravelerExtraDetailForm updatedForm)
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var form = await _travelerRepository.GetByIdAsync(localId, userId);

                if (form == null)
                {
                    updatedForm.FormId = FormId;
                    updatedForm.TravelerId = userId;
                    updatedForm.CreateById = localId;
                    updatedForm.SendAt = GetTimeZone.GetVNTimeZoneNow();
                    await _travelerRepository.AddAsync(updatedForm);
                    return Ok("Created successfully!");
                }

                if (form != null && form.RequestStatus)
                    return BadRequest("Request was processed! You can not update");

                form.LatestUpdateAt = GetTimeZone.GetVNTimeZoneNow();
                form.Services = updatedForm.Services;
                form.Questions = updatedForm.Questions;
                form.StartDate = updatedForm.StartDate;
                form.EndDate = updatedForm.EndDate;

                await _travelerRepository.UpdateAsync(FormId, form);

                return Ok("Update form request successfully!");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while updating the traveler form.", Details = ex.Message });
            }
        }


        // DELETE: api/TravelerForm/{FormId}
        [HttpDelete("TravelerForm/{FormId}")]
        public async Task<IActionResult> DeleteTravelerForm(string FormId, [FromQuery] int localId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                // Retrieve the form using FormId and associated localId & travelerId (userId)
                var form = await _travelerRepository.GetByIdAsync(localId, userId);

                if (form == null)
                {
                    return NotFound(new { Message = "Form not found." });
                }

                if (form.RequestStatus)
                {
                    return BadRequest("Request was processed! You cannot delete.");
                }

                // Delete the form
                await _travelerRepository.DeleteAsync(FormId);
                return Ok("Form deleted successfully!");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while deleting the traveler form.", Details = ex.Message });
            }
        }

    }
}
