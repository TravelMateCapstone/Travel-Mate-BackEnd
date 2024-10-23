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
    public class UniversityRepository : IUniversityRepository
    {
        private readonly UniversityDAO _universityDAO;

        public UniversityRepository(UniversityDAO universityDAO)
        {
            _universityDAO = universityDAO;
        }

        public async Task<List<University>> GetAllUniversitiesAsync()
        {
            return await _universityDAO.GetAllUniversitiesAsync();
        }

        public async Task<University> GetUniversityByIdAsync(int universityId)
        {
            return await _universityDAO.GetUniversityByIdAsync(universityId);
        }

        public async Task<University> AddUniversityAsync(University newUniversity)
        {
            return await _universityDAO.AddUniversityAsync(newUniversity);
        }

        public async Task UpdateUniversityAsync(University updatedUniversity)
        {
            await _universityDAO.UpdateUniversityAsync(updatedUniversity);
        }

        public async Task DeleteUniversityAsync(int universityId)
        {
            await _universityDAO.DeleteUniversityAsync(universityId);
        }
    }

}
