using BussinessObjects.Entities;
using BussinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class LanguagesDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public LanguagesDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Languages>> GetAllLanguagesAsync()
        {
            return await _dbContext.Languages.ToListAsync();
        }

        public async Task<Languages> GetLanguagesByIdAsync(int languagesId)
        {
            return await _dbContext.Languages.FindAsync(languagesId);
        }

        public async Task<Languages> AddLanguagesAsync(Languages newLanguages)
        {
            _dbContext.Languages.Add(newLanguages);
            await _dbContext.SaveChangesAsync();
            return newLanguages;
        }

        public async Task UpdateLanguagesAsync(Languages updatedLanguages)
        {
            _dbContext.Languages.Update(updatedLanguages);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLanguagesAsync(int languagesId)
        {
            var languages = await GetLanguagesByIdAsync(languagesId);
            if (languages != null)
            {
                _dbContext.Languages.Remove(languages);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
