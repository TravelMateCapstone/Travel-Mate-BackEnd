using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITourParticipantRepository _tourParticipantRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ITransactionRepository transactionRepository, UserManager<ApplicationUser> userManager, ITourParticipantRepository tourParticipantRepository)
        {
            _transactionRepository = transactionRepository;
            _userManager = userManager;
            _tourParticipantRepository = tourParticipantRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        //them dieu kien
        [HttpPost("completePayment")]
        public async Task<ActionResult> CompletePayment([FromQuery] string transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //kiem tra trang thai payment

            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);

            var timeNow = GetTimeZone.GetVNTimeZoneNow();
            if (timeNow <= transaction.EndDate)
                return BadRequest("Access Denied! Tour does not finish!");

            await _transactionRepository.CompletePaymentStatus(transactionId);

            return Ok();
        }

        [HttpPost("completeRefund")]
        public async Task<ActionResult> CompleteRefund([FromQuery] string transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);

            await _transactionRepository.CompleteRefundStatus(transactionId);
            await _tourParticipantRepository.UpdateRefundDone(transaction.TourId, transaction.ScheduleId, transaction.ParticipantId);

            return Ok();
        }

    }
}
