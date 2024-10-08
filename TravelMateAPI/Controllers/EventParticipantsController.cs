using BussinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    //[ApiController]
    //[Route("odata/[controller]")]
    public class EventParticipantsController : ODataController
    {
        private readonly IEventParticipantsRepository _participantsRepository;

        public EventParticipantsController(IEventParticipantsRepository participantsRepository)
        {
            _participantsRepository = participantsRepository;
        }

        // GET: odata/EventParticipants
        [EnableQuery] // Kích hoạt OData cho truy vấn
        public async Task<IActionResult> Get(ODataQueryOptions<ApplicationUser> queryOptions)
        {
            var participants = await _participantsRepository.GetAllParticipantsAsync();
            return Ok(participants);
        }

        /*public IActionResult GetAll()
        {
            var participants = _participantsRepository.GetAllParticipantsAsync().Result.AsQueryable();
            return Ok(participants);
        }*/

        // GET: odata/EventParticipants(1)
        [EnableQuery] // Kích hoạt OData cho truy vấn theo ID
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var participant = await _participantsRepository.GetParticipantByIdAsync(key);
            if (participant == null)
            {
                return NotFound();
            }
            return Ok(participant);
        }

        // POST: odata/EventParticipants
        public async Task<IActionResult> Post([FromBody] EventParticipants participant)
        {
            var createdParticipant = await _participantsRepository.AddParticipantAsync(participant);
            return Created(createdParticipant);
        }

        // PUT: odata/EventParticipants(1)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] EventParticipants participant)
        {
            if (key != participant.EventParticipantId)
            {
                return BadRequest();
            }
            await _participantsRepository.UpdateParticipantAsync(participant);
            return NoContent();
        }

        // DELETE: odata/EventParticipants(1)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            await _participantsRepository.DeleteParticipantAsync(key);
            return NoContent();
        }
    }
}
