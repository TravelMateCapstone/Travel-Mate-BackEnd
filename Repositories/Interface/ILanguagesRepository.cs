using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ILanguagesRepository
    {
        Task<List<Languages>> GetAllLanguagesAsync();
        Task<Languages> GetLanguagesByIdAsync(int languagesId);
        Task<Languages> AddLanguagesAsync(Languages newLanguages);
        Task UpdateLanguagesAsync(Languages updatedLanguages);
        Task DeleteLanguagesAsync(int languagesId);
    }

}
