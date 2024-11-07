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
    public class UserHomeDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public UserHomeDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserHome>> GetAllUserHomesAsync()
        {
            return await _dbContext.UserHomes
                           .Include(uh => uh.ApplicationUser)
                           .ThenInclude(u => u.Profiles) // Thêm Include cho Profile của ApplicationUser
                           .Include(uh => uh.HomePhotos) // Lấy kèm HomePhotos nếu cần
                           .ToListAsync();
            //return await _dbContext.UserHomes.Include(uh => uh.ApplicationUser).ToListAsync();
        }


        public async Task<UserHome> GetUserHomeByIdAsync(int userHomeId)
        {
            return await _dbContext.UserHomes.Include(uh => uh.ApplicationUser)
                                             .FirstOrDefaultAsync(uh => uh.UserHomeId == userHomeId);
        }

        public async Task<UserHome> GetUserHomeByUserIdAsync(int userId)
        {
            return await _dbContext.UserHomes.Include(uh => uh.ApplicationUser).Include(uh => uh.HomePhotos)
                                             .FirstOrDefaultAsync(uh => uh.UserId == userId);
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

        public async Task DeleteUserHomeAsync(int userHomeId)
        {
            var userHome = await GetUserHomeByIdAsync(userHomeId);
            if (userHome != null)
            {
                _dbContext.UserHomes.Remove(userHome);
                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
