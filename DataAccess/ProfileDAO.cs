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
    public class ProfileDAO : SingletonBase<ProfileDAO>
    {
        private readonly ApplicationDBContext _dbContext;

        public ProfileDAO()
        {
            _dbContext = SingletonBase<ProfileDAO>._context;
        }

        public async Task<IEnumerable<Profile>> GetAllProfilesAsync()
        {
            return await _dbContext.Profiles.ToListAsync();
        }

        public async Task<Profile> GetProfileByIdAsync(string userId)
        {
            return await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Profile> AddProfileAsync(Profile newProfile)
        {
            _dbContext.Profiles.Add(newProfile);
            await _dbContext.SaveChangesAsync();
            return newProfile;
        }

        public async Task UpdateProfileAsync(Profile updatedProfile)
        {
            _dbContext.Profiles.Update(updatedProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProfileAsync(string userId)
        {
            var profile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile != null)
            {
                _dbContext.Profiles.Remove(profile);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
