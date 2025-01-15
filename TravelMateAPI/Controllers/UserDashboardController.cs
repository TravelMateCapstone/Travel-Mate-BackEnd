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
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDashboardController(UserManager<ApplicationUser> userManager, ITourParticipantRepository transactionRepository, ITourRepository tourRepository)
        {
            _transactionRepository = transactionRepository;
            _userManager = userManager;
            _tourRepository = tourRepository;
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
                .Where(t => t.PaymentStatus == PaymentStatus.Success)
                .ToList();

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

            var info = new
            {
                Revenue = (double)successfulTransactions.Sum(t => t.TotalAmount ?? 0) * 0.9,
                TotalAmount = (double)successfulTransactions.Sum(t => t.TotalAmount ?? 0),
                TotalTour = successfulTransactions.Count(),
                TotalTraveler = totalTraveler,
                MonthlyRevenues = monthlyRevenues
            };

            return Ok(info);
        }
    }
}
