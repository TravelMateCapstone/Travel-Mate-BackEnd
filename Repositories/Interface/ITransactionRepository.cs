using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITransactionRepository
    {
        Task<List<TourTransaction>> GetAllTransactionsAsync();
        Task<TourTransaction?> GetTransactionByIdAsync(string id);
        Task AddTransactionAsync(TourTransaction transaction);
    }
}
