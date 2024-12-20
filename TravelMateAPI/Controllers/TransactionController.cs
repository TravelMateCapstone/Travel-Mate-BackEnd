using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("traveler/{id}")]
        public async Task<IActionResult> GetTransactionsByTravelerId(int id)
        {
            var transactions = await _transactionRepository.GetTransactionByIdAsync(id);
            return Ok(transactions);
        }

    }
}
