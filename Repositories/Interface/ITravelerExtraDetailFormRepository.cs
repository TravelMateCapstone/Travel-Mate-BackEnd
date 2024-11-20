using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITravelerFormRepository
    {
        Task<IEnumerable<TravelerExtraDetailForm>> GetAllAsync();
        Task<TravelerExtraDetailForm> GetByIdAsync(int localId, int travelerId);
        Task<IEnumerable<TravelerExtraDetailForm>> GetByTravelerIdAsync(int travelerId);
        Task AddAsync(TravelerExtraDetailForm form);
        Task UpdateAsync(string formId, TravelerExtraDetailForm updatedForm);

        Task DeleteAsync(string formId);

    }
}
