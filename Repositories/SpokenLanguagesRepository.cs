using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SpokenLanguagesRepository : ISpokenLanguagesRepository
    {
        private readonly SpokenLanguagesDAO _spokenLanguagesDAO;

        public SpokenLanguagesRepository(SpokenLanguagesDAO spokenLanguagesDAO)
        {
            _spokenLanguagesDAO = spokenLanguagesDAO;
        }

        public async Task<List<SpokenLanguages>> GetAllSpokenLanguagesAsync()
        {
            return await _spokenLanguagesDAO.GetAllSpokenLanguagesAsync();
        }

        public async Task<SpokenLanguages> GetSpokenLanguageByIdAsync(int languagesId, int userId)
        {
            return await _spokenLanguagesDAO.GetSpokenLanguageByIdAsync(languagesId, userId);
        }

        public async Task<SpokenLanguages> AddSpokenLanguageAsync(SpokenLanguages newSpokenLanguage)
        {
            return await _spokenLanguagesDAO.AddSpokenLanguageAsync(newSpokenLanguage);
        }

        public async Task UpdateSpokenLanguageAsync(SpokenLanguages updatedSpokenLanguage)
        {
            await _spokenLanguagesDAO.UpdateSpokenLanguageAsync(updatedSpokenLanguage);
        }

        public async Task DeleteSpokenLanguageAsync(int languagesId, int userId)
        {
            await _spokenLanguagesDAO.DeleteSpokenLanguageAsync(languagesId, userId);
        }
    }


}
