using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
namespace DataAccess
{
    public class UserEducationDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public UserEducationDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserEducation>> GetAllUserEducationsAsync()
        {
            return await _dbContext.UserEducations.Include(ue => ue.University).Include(ue => ue.ApplicationUser).ToListAsync();
        }

        public async Task<UserEducation> GetUserEducationByIdAsync(int universityId, int userId)
        {
            return await _dbContext.UserEducations
                .Include(ue => ue.University)
                .Include(ue => ue.ApplicationUser)
                .FirstOrDefaultAsync(ue => ue.UniversityId == universityId && ue.UserId == userId);
        }

        public async Task<UserEducation> AddUserEducationAsync(UserEducation newUserEducation)
        {
            _dbContext.UserEducations.Add(newUserEducation);
            await _dbContext.SaveChangesAsync();
            return newUserEducation;
        }

        public async Task UpdateUserEducationAsync(UserEducation updatedUserEducation)
        {
            _dbContext.UserEducations.Update(updatedUserEducation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserEducationAsync(int universityId, int userId)
        {
            var userEducation = await GetUserEducationByIdAsync(universityId, userId);
            if (userEducation != null)
            {
                _dbContext.UserEducations.Remove(userEducation);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
