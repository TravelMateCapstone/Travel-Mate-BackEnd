using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TourTransaction>> GetAllTransactionsAsync();
        Task<TourTransaction?> GetTransactionByIdAsync(string id);
        Task AddTransactionAsync(TourTransaction transaction);

        Task UpdateRefundStatus(string scheduleId, int travelerId);
        Task CompleteRefundStatus(string transactionId);

        Task CompletePaymentStatus(string transactionId);
    }
}
