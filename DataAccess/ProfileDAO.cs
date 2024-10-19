using BussinessObjects;
using BussinessObjects.Entities;
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

        public async Task<List<UserProfile>> GetAllProfilesAsync()
        {
            return await _dbContext.Profiles.ToListAsync();
        }

        public async Task<UserProfile> GetProfileByIdAsync(int userId)
        {
            return await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<UserProfile> AddProfileAsync(UserProfile newProfile)
        {
            _dbContext.Profiles.Add(newProfile);
            await _dbContext.SaveChangesAsync();
            return newProfile;
        }

        public async Task UpdateProfileAsync(UserProfile updatedProfile)
        {
            _dbContext.Profiles.Update(updatedProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProfileAsync(int userId)
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
