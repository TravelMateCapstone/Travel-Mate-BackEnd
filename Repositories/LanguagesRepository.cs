using BussinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LanguagesRepository : ILanguagesRepository
    {
        private readonly LanguagesDAO _languagesDAO;

        public LanguagesRepository(LanguagesDAO languagesDAO)
        {
            _languagesDAO = languagesDAO;
        }

        public async Task<List<Languages>> GetAllLanguagesAsync()
        {
            return await _languagesDAO.GetAllLanguagesAsync();
        }

        public async Task<Languages> GetLanguagesByIdAsync(int languagesId)
        {
            return await _languagesDAO.GetLanguagesByIdAsync(languagesId);
        }

        public async Task<Languages> AddLanguagesAsync(Languages newLanguages)
        {
            return await _languagesDAO.AddLanguagesAsync(newLanguages);
        }

        public async Task UpdateLanguagesAsync(Languages updatedLanguages)
        {
            await _languagesDAO.UpdateLanguagesAsync(updatedLanguages);
        }

        public async Task DeleteLanguagesAsync(int languagesId)
        {
            await _languagesDAO.DeleteLanguagesAsync(languagesId);
        }
    }

}
