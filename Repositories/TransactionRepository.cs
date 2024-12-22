using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDAO _transactionDAO;

        public TransactionRepository(TransactionDAO transactionDAO)
        {
            _transactionDAO = transactionDAO;
        }
        public async Task<IEnumerable<TourTransaction>> GetAllTransactionsAsync()
        {
            return await _transactionDAO.GetAllTransactionsAsync();
        }

        public async Task<IEnumerable<TourTransaction>> GetTransactionByIdAsync(int id)
        {
            return await _transactionDAO.GetTransactionByIdAsync(id);
        }

        public async Task AddTransactionAsync(TourTransaction transaction)
        {

            await _transactionDAO.AddTransactionAsync(transaction);
        }
    }
}
