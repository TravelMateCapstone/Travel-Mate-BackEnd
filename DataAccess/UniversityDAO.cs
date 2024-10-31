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
    public class UniversityDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public UniversityDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<University>> GetAllUniversitiesAsync()
        {
            return await _dbContext.Universities.ToListAsync();
        }

        public async Task<University> GetUniversityByIdAsync(int universityId)
        {
            return await _dbContext.Universities.FindAsync(universityId);
        }

        public async Task<University> AddUniversityAsync(University newUniversity)
        {
            _dbContext.Universities.Add(newUniversity);
            await _dbContext.SaveChangesAsync();
            return newUniversity;
        }

        public async Task UpdateUniversityAsync(University updatedUniversity)
        {
            _dbContext.Universities.Update(updatedUniversity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUniversityAsync(int universityId)
        {
            var university = await GetUniversityByIdAsync(universityId);
            if (university != null)
            {
                _dbContext.Universities.Remove(university);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
