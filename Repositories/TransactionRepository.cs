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

        public async Task<TourTransaction> GetTransactionByIdAsync(string id)
        {
            return await _transactionDAO.GetTransactionByIdAsync(id);
        }

        public async Task AddTransactionAsync(TourTransaction transaction)
        {
            await _transactionDAO.AddTransactionAsync(transaction);
        }

        public async Task UpdateRefundStatus(string scheduleId, int travelerId)
        {
            var transaction = await _transactionDAO.getTransactionByScheduleId(scheduleId, travelerId);

            transaction.TransactionStatus = BusinessObjects.EnumClass.PaymentStatus.ProcessRefund;

            await _transactionDAO.UpdateTransaction(transaction.Id, transaction);
        }

        public async Task CompleteRefundStatus(string transactionId)
        {
            var transaction = await _transactionDAO.GetTransactionByIdAsync(transactionId);

            transaction.TransactionStatus = BusinessObjects.EnumClass.PaymentStatus.Refund;

            await _transactionDAO.UpdateTransaction(transaction.Id, transaction);
        }

        public async Task CompletePaymentStatus(string transactionId)
        {
            var transaction = await _transactionDAO.GetTransactionByIdAsync(transactionId);

            transaction.TransactionStatus = BusinessObjects.EnumClass.PaymentStatus.Success;

            await _transactionDAO.UpdateTransaction(transaction.Id, transaction);
        }
    }
}
