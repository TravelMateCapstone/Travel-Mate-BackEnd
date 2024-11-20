using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class LocalExtraDetailFormRepository : ILocalExtraDetailFormRepository
    {
        private readonly ExtraFormDetailDAO _extraFormDetailDAO;

        public LocalExtraDetailFormRepository(ExtraFormDetailDAO extraFormDetailDAO)
        {
            _extraFormDetailDAO = extraFormDetailDAO;
        }

        public async Task<LocalExtraDetailForm> GetByIdAsync(string FormId)
        {
            return await _extraFormDetailDAO.GetById(FormId);
        }

        public async Task<IEnumerable<LocalExtraDetailForm>> GetAllAsync()
        {
            return await _extraFormDetailDAO.GetAlls();
        }

        public async Task CreateAsync(LocalExtraDetailForm form)
        {
            await _extraFormDetailDAO.AddAsync(form);
        }

        public async Task UpdateAsync(string id, LocalExtraDetailForm updatedForm)
        {
            await _extraFormDetailDAO.UpdateAsync(id, updatedForm);
        }

        public async Task<LocalExtraDetailForm> GetByUserIdAsync(int UserId)
        {
            return await _extraFormDetailDAO.GetByUserId(UserId);
        }
    }
}
