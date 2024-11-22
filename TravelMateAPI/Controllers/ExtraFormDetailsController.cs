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
                existingForm.LatestUpdateAt = GetTimeZone.GetVNTimeZoneNow();

                await _localRepository.UpdateAsync(existingForm.Id, existingForm);
                return Ok("Update successfully");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while updating the form.", Details = ex.Message });
            }
        }

        //----------------------------------TRAVELER

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

                // Retrieve the LocalExtraDetailForm
                var localForm = await _localRepository.GetByUserIdAsync(localId);

                // Retrieve the existing TravelerExtraDetailForm
                var existingFormRequest = await _travelerRepository.GetByIdAsync(localId, userId);
                if (existingFormRequest != null)
                {
                    if (localForm.LatestUpdateAt > existingFormRequest.SendAt)
                    {
                        //mapping cau hoi
                        existingFormRequest = _mapper.Map<TravelerExtraDetailForm>(localForm);
                        //kiem tra su thay doi o cac cau hoi

                        foreach (var item in existingFormRequest.AnsweredQuestions)
                        {
                            existingFormRequest.AnsweredQuestions.Remove(item);
                        }

                        foreach (var item in localForm.Questions)
                        {
                            var answer = new AnsweredQuestion()
                            {
                                QuestionId = item.Id,
                                Answer = new List<string>()
                            };
                            existingFormRequest.AnsweredQuestions.Add(answer);
                        }

                        foreach (var item in localForm.Services)
                        {
                            var answer = new AnsweredService()
                            {
                                ServiceId = item.Id,
                                Total = 0
                            };
                            existingFormRequest.AnsweredServices.Add(answer);
                        }

                    }
                    return Ok(existingFormRequest);
                }

                var newForm = _mapper.Map<TravelerExtraDetailForm>(localForm);

                foreach (var item in localForm.Questions)
                {
                    var answer = new AnsweredQuestion()
                    {
                        QuestionId = item.Id,
                        Answer = new List<string>()
                    };
                    newForm.AnsweredQuestions.Add(answer);
                }

                foreach (var item in localForm.Services)
                {
                    var answer = new AnsweredService()
                    {
                        ServiceId = item.Id,
                        Total = 0
                    };
                    newForm.AnsweredServices.Add(answer);
                }

                return Ok(newForm);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while processing the form request.", Details = ex.Message });
            }
        }

        // PUT: api/TravelerForm/{FormId}
        [HttpPut("TravelerForm")]
        public async Task<IActionResult> UpdateTravelerForm([FromQuery] int localId, [FromBody] TravelerExtraDetailForm updatedForm)
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var form = await _travelerRepository.GetByIdAsync(localId, userId);
                var localForm = await _localRepository.GetByUserIdAsync(localId);
                if (userId == localId)
                    return BadRequest("You can not request yourself!");

                //gan cau tra loi tuong ung voi cau hoi (gan questionId, serviceId)
                if (form == null)
                {
                    updatedForm.TravelerId = userId;
                    updatedForm.CreateById = localId;
                    updatedForm.SendAt = GetTimeZone.GetVNTimeZoneNow();
                    updatedForm.Questions = localForm.Questions;
                    updatedForm.Services = localForm.Services;
                    await _travelerRepository.AddAsync(updatedForm);
                    return Ok("Created successfully!");
                }

                if (form != null && form.RequestStatus)
                    return BadRequest("Request was processed! You can not update");

                form.LatestUpdateAt = GetTimeZone.GetVNTimeZoneNow();
                form.StartDate = updatedForm.StartDate;
                form.EndDate = updatedForm.EndDate;
                form.Questions = localForm.Questions;
                form.Services = localForm.Services;
                form.AnsweredQuestions = updatedForm.AnsweredQuestions;
                form.AnsweredServices = updatedForm.AnsweredServices;

                //update form of local and user
                await _travelerRepository.UpdateAsync(localId, userId, form);

                return Ok("Update form request successfully!");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { Message = "An error occurred while updating the traveler form.", Details = ex.Message });
            }
        }


        // DELETE: api/TravelerForm/{FormId}
        [HttpDelete("TravelerForm")]
        public async Task<IActionResult> DeleteTravelerForm([FromQuery] int localId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == -1)
                    return Unauthorized(new { Message = "Unauthorized access." });

                // Retrieve the form using FormId and associated localId & travelerId (userId)
                var form = await _travelerRepository.GetByIdAsync(localId, userId);

                if (userId == form.CreateById)
                    return BadRequest("You can not delete request form of other");

                if (form == null)
                {
                    return NotFound(new { Message = "Form not found." });
                }

                if (form.RequestStatus)
                {
                    return BadRequest("Request was processed! You cannot delete.");
                }

                // Delete the form of local and traveler
                await _travelerRepository.DeleteAsync(localId, userId);
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
