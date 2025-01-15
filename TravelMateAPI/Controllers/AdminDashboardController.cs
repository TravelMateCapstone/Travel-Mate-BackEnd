using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ITransactionRepository _transactionRepository;

        public AdminDashboardController(ApplicationDBContext dbContext, ITransactionRepository transactionRepository)
        {
            _dbContext = dbContext;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardInfo()
        {
            var revenue = await _transactionRepository.GetAllTransactionsAsync();
            var userCount = _dbContext.Users.Count();
            var reportCount = _dbContext.Reports.Count();

            var transactions2025 = revenue
    .Where(t => t.TransactionTime.HasValue && t.TransactionTime.Value.Year == 2025)
    .ToList();


            var monthlyRevenues = Enumerable.Range(1, 12)
                .Select(month => new
                {
                    Month = month,
                    Revenue = transactions2025
                        .Where(t => t.TransactionTime.HasValue && t.TransactionTime.Value.Month == month)
                        .Sum(t => t.Amount)
                })
                .ToList();

            var info = new
            {
                Revenue = (double)revenue.Average(t => t.Amount) * 0.1,
                TotalTrips = revenue.Count(),
                TotalUsers = userCount,
                TotalReports = reportCount,
                MonthlyRevenues = monthlyRevenues
            };

            return Ok(info);
        }

    }
}
