using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserDashboardController : ControllerBase
    {
        private readonly ITourParticipantRepository _transactionRepository;
        private readonly ITourRepository _tourRepository;
        private readonly ITransactionRepository _localTransactionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDashboardController(UserManager<ApplicationUser> userManager, ITourParticipantRepository transactionRepository, ITourRepository tourRepository, ITransactionRepository localTransaction)
        {
            _transactionRepository = transactionRepository;
            _userManager = userManager;
            _tourRepository = tourRepository;
            _localTransactionRepository = localTransaction;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var totalTraveler = 0;
            var allTransactions = await _transactionRepository.GetAllTransactionsAsync();

            var receivedTransactions = await _localTransactionRepository.GetAllTransactionsAsync();

            // Filter transactions where the logged-in user is the creator of the tour
            var validTransactions = new List<TravelerTransaction>();

            foreach (var transaction in allTransactions)
            {
                var tour = await _tourRepository.GetTourById(transaction.TourId);
                if (tour != null && tour.Creator.Id == user.Id)
                {
                    validTransactions.Add(transaction);
                    totalTraveler += 1;
                }
            }

            // Further filter transactions with PaymentStatus = Success
            var successfulTransactions = validTransactions
                .Where(t => t.PaymentStatus == PaymentStatus.Success || t.PaymentStatus == PaymentStatus.ProcessRefund)
                .ToList();

            // Calculate ReceivedAmount from transactions with PaymentStatus = Success
            var receivedAmount = receivedTransactions
                .Where(t => t.TransactionStatus == PaymentStatus.Success && t.LocalId == user.Id)
                .Sum(t => t.LastAmount ?? 0);

            var transaction2025 = successfulTransactions
                .Where(t => t.TransactionTime.HasValue && t.TransactionTime.Value.Year == 2025)
                .ToList();

            var monthlyRevenues = Enumerable.Range(1, 12)
                .Select(month => new
                {
                    Month = month,
                    Revenue = transaction2025
                        .Where(t => t.TransactionTime.HasValue && t.TransactionTime.Value.Month == month)
                        .Sum(t => t.TotalAmount ?? 0)
                })
                .ToList();

            var totalTour = successfulTransactions
                .Select(t => t.ScheduleId)
                .Distinct()
                .Count();

            var info = new
            {
                ReceivedAmount = receivedAmount,
                Revenue = (double)successfulTransactions.Sum(t => t.TotalAmount ?? 0) * 0.9,
                TotalAmount = (double)successfulTransactions.Sum(t => t.TotalAmount ?? 0),
                TotalTour = totalTour,
                TotalTraveler = totalTraveler,
                MonthlyRevenues = monthlyRevenues
            };

            return Ok(info);
        }

    }
}
