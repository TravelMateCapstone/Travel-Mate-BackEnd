using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TourTransaction>> GetAllTransactionsAsync();
        Task<IEnumerable<TourTransaction?>> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(TourTransaction transaction);
    }
}
