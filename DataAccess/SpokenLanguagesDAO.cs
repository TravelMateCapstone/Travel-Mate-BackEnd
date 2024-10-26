using BusinessObjects.Entities;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SpokenLanguagesDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public SpokenLanguagesDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SpokenLanguages>> GetAllSpokenLanguagesAsync()
        {
            return await _dbContext.SpokenLanguages.Include(sl => sl.Languages).Include(sl => sl.ApplicationUser).ToListAsync();
        }

        public async Task<SpokenLanguages> GetSpokenLanguageByIdAsync(int languagesId, int userId)
        {
            return await _dbContext.SpokenLanguages
                .Include(sl => sl.Languages)
                .Include(sl => sl.ApplicationUser)
                .FirstOrDefaultAsync(sl => sl.LanguagesId == languagesId && sl.UserId == userId);
        }
        public async Task<List<SpokenLanguages>> GetSpokenLanguagesByUserIdAsync(int userId)
        {
            return await _dbContext.SpokenLanguages.Include(sl => sl.Languages)
                                                    .Include(sl => sl.ApplicationUser)
                                                    .Where(sl => sl.UserId == userId)
                                                    .ToListAsync();
        }
        public async Task<SpokenLanguages> AddSpokenLanguageAsync(SpokenLanguages newSpokenLanguage)
        {
            _dbContext.SpokenLanguages.Add(newSpokenLanguage);
            await _dbContext.SaveChangesAsync();
            return newSpokenLanguage;
        }

        public async Task UpdateSpokenLanguageAsync(SpokenLanguages updatedSpokenLanguage)
        {
            _dbContext.SpokenLanguages.Update(updatedSpokenLanguage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSpokenLanguageAsync(int languagesId, int userId)
        {
            var spokenLanguage = await GetSpokenLanguageByIdAsync(languagesId, userId);
            if (spokenLanguage != null)
            {
                _dbContext.SpokenLanguages.Remove(spokenLanguage);
                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
