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
    public class UserHomeDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public UserHomeDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserHome>> GetAllUserHomesAsync()
        {
            return await _dbContext.UserHomes.Include(u => u.ApplicationUser).ToListAsync();
        }

        public async Task<UserHome> GetUserHomeByIdAsync(int userId)
        {
            return await _dbContext.UserHomes.Include(u => u.ApplicationUser).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserHome> AddUserHomeAsync(UserHome newUserHome)
        {
            _dbContext.UserHomes.Add(newUserHome);
            await _dbContext.SaveChangesAsync();
            return newUserHome;
        }

        public async Task UpdateUserHomeAsync(UserHome updatedUserHome)
        {
            _dbContext.UserHomes.Update(updatedUserHome);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserHomeAsync(int userId)
        {
            var userHome = await GetUserHomeByIdAsync(userId);
            if (userHome != null)
            {
                _dbContext.UserHomes.Remove(userHome);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
