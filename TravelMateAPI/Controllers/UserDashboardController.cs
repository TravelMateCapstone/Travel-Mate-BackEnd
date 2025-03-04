﻿using BusinessObjects.Entities;
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

            // Filter transactions with PaymentStatus = Success or ProcessRefund addf
            var successfulTransactions = validTransactions
                .Where(t => t.PaymentStatus == PaymentStatus.Success)
                .ToList();

            var refundTransactions = validTransactions
                .Where(t => t.PaymentStatus == PaymentStatus.ProcessRefund)
                .ToList();

            // Calculate ReceivedAmount from transactions with PaymentStatus = Success
            var receivedAmount = receivedTransactions
                .Where(t => t.TransactionStatus == PaymentStatus.Success && t.LocalId == user.Id)
                .Sum(t => t.LastAmount ?? 0);

            // Include both successful and refund transactions for totals and revenue
            var combinedTransactions = successfulTransactions.Concat(refundTransactions).ToList();

            var transaction2025 = combinedTransactions
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

            var totalTour = combinedTransactions
                .Select(t => t.ScheduleId)
                .Distinct()
                .Count();

            var totalAmount = (double)combinedTransactions.Sum(t => t.TotalAmount ?? 0);
            var revenue = totalAmount * 0.9;

            var info = new
            {
                ReceivedAmount = receivedAmount,
                Revenue = revenue,
                TotalAmount = totalAmount,
                TotalTour = totalTour,
                TotalTraveler = totalTraveler,
                MonthlyRevenues = monthlyRevenues
            };

            return Ok(info);
        }

    }
}
