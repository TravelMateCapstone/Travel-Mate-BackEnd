using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ISpokenLanguagesRepository
    {
        Task<List<SpokenLanguages>> GetAllSpokenLanguagesAsync();
        Task<SpokenLanguages> GetSpokenLanguageByIdAsync(int languagesId, int userId);
        Task<SpokenLanguages> AddSpokenLanguageAsync(SpokenLanguages newSpokenLanguage);
        Task UpdateSpokenLanguageAsync(SpokenLanguages updatedSpokenLanguage);
        Task DeleteSpokenLanguageAsync(int languagesId, int userId);
    }


}
