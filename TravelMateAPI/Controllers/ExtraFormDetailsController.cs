using AutoMapper;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ExtraFormDetailsController(UserManager<ApplicationUser> userManager, ILocalExtraDetailFormRepository localExtraDetailFormRepository, ITravelerFormRepository travelerRepository, IMapper mapper)
        {
            _userManager = userManager;
            _localRepository = localExtraDetailFormRepository;
            _travelerRepository = travelerRepository;
            _mapper = mapper;
        }

        [HttpGet("Request")]
        public async Task<IActionResult> GetListRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var form = await _travelerRepository.GetAllRequests(user.Id);
            return Ok(form);
        }

        [HttpPost("AcceptRequest")]
        public async Task<IActionResult> AcceptRequest([FromQuery] int travelerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (travelerId == user.Id)
                return BadRequest("Access Denied! You can not process your request to other");

            var existingForm = await _travelerRepository.GetByIdAsync(user.Id, travelerId);
            existingForm.RequestStatus = true;
            await _travelerRepository.ProcessRequest(existingForm);

            return Ok("Accepting request");
        }

        [HttpPost("RejectRequest")]
        public async Task<IActionResult> RejectRequest([FromQuery] int travelerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (travelerId == user.Id)
                return BadRequest("Access Denied! You can not process your request to other");

            var existingForm = await _travelerRepository.GetByIdAsync(user.Id, travelerId);
            existingForm.RequestStatus = false;
            await _travelerRepository.ProcessRequest(existingForm);

            return Ok("Accepting request");
        }

        [HttpGet("Chats")]
        public async Task<IActionResult> GetListChats()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var form = await _travelerRepository.GetAllChats(user.Id);

            return Ok(form);
        }

        [HttpGet("LocalForm")]
        public async Task<IActionResult> GetFormByUserId()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var form = await _localRepository.GetByUserIdAsync(user.Id);
            return Ok(form);
        }

        [HttpPut("LocalForm")]
        public async Task<IActionResult> UpdateForm([FromBody] LocalExtraDetailForm updatedForm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existingForm = await _localRepository.GetByUserIdAsync(user.Id);
            if (existingForm == null)
            {
                updatedForm.CreateById = user.Id;
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

        //TRAVELER

        [HttpGet("TravelerForm")]
        public async Task<IActionResult> GetFormRequest([FromQuery] int localId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (user.Id == localId)
                return BadRequest("You can not request yourself!");

            var localForm = await _localRepository.GetByUserIdAsync(localId);

            var existingFormRequest = await _travelerRepository.GetByIdAsync(localId, user.Id);
            if (existingFormRequest != null)
            {
                if (localForm.LatestUpdateAt > existingFormRequest.SendAt)
                {
                    existingFormRequest = _mapper.Map<TravelerExtraDetailForm>(localForm);
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

        [HttpPut("TravelerForm")]
        public async Task<IActionResult> UpdateTravelerForm([FromQuery] int localId, [FromBody] TravelerExtraDetailForm updatedForm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var form = await _travelerRepository.GetByIdAsync(localId, user.Id);
            var localForm = await _localRepository.GetByUserIdAsync(localId);
            if (user.Id == localId)
                return BadRequest("You can not request yourself!");

            if (form == null)
            {
                updatedForm.TravelerId = user.Id;
                updatedForm.CreateById = localId;
                updatedForm.SendAt = GetTimeZone.GetVNTimeZoneNow();
                updatedForm.Questions = localForm.Questions;
                updatedForm.Services = localForm.Services;
                await _travelerRepository.AddAsync(updatedForm);
                return Ok("Created successfully!");
            }

            if (form != null && form.RequestStatus == true)
                return BadRequest("Request was processed! You can not update");

            form.LatestUpdateAt = GetTimeZone.GetVNTimeZoneNow();
            form.StartDate = updatedForm.StartDate;
            form.EndDate = updatedForm.EndDate;
            form.Questions = localForm.Questions;
            form.Services = localForm.Services;
            form.AnsweredQuestions = updatedForm.AnsweredQuestions;
            form.AnsweredServices = updatedForm.AnsweredServices;

            await _travelerRepository.UpdateAsync(localId, user.Id, form);

            return Ok("Update form request successfully!");
        }

        [HttpDelete("TravelerForm")]
        public async Task<IActionResult> DeleteTravelerForm([FromQuery] int localId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var form = await _travelerRepository.GetByIdAsync(localId, user.Id);

            if (user.Id == form.CreateById)
                return BadRequest("You can not delete request form of other");

            if (form == null)
            {
                return NotFound(new { Message = "Form not found." });
            }

            if (form.RequestStatus == true)
            {
                return BadRequest("Request was processed! You cannot delete.");
            }

            await _travelerRepository.DeleteAsync(localId, user.Id);

            return Ok("Form deleted successfully!");
        }

    }
}
