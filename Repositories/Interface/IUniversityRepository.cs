using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUniversityRepository
    {
        Task<List<University>> GetAllUniversitiesAsync();
        Task<University> GetUniversityByIdAsync(int universityId);
        Task<University> AddUniversityAsync(University newUniversity);
        Task UpdateUniversityAsync(University updatedUniversity);
        Task DeleteUniversityAsync(int universityId);
    }

}
