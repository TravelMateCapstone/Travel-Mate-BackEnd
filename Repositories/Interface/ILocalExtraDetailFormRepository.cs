using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ILocalExtraDetailFormRepository
    {
        Task<LocalExtraDetailForm> GetByIdAsync(string FormId);
        Task<LocalExtraDetailForm> GetByUserIdAsync(int UserId);
        Task<IEnumerable<LocalExtraDetailForm>> GetAllAsync();
        Task CreateAsync(LocalExtraDetailForm form);
        Task UpdateAsync(string id, LocalExtraDetailForm updatedForm);
    }
}
