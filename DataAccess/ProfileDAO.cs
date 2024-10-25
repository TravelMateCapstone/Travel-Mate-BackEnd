using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    //public class ProfileDAO : SingletonBase<ProfileDAO>
    //{
    //    private readonly ApplicationDBContext _dbContext;

    //    public ProfileDAO()
    //    {
    //        _dbContext = SingletonBase<ProfileDAO>._context;
    //    }

    //    public async Task<List<Profile>> GetAllProfilesAsync()
    //    {
    //        return await _dbContext.Profiles.ToListAsync();
    //    }

    //    public async Task<Profile> GetProfileByIdAsync(int userId)
    //    {
    //        return await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
    //    }

    //    public async Task<Profile> AddProfileAsync(Profile newProfile)
    //    {
    //        _dbContext.Profiles.Add(newProfile);
    //        await _dbContext.SaveChangesAsync();
    //        return newProfile;
    //    }

    //    public async Task UpdateProfileAsync(Profile updatedProfile)
    //    {
    //        _dbContext.Profiles.Update(updatedProfile);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    public async Task DeleteProfileAsync(int userId)
    //    {
    //        var profile = await _dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
    //        if (profile != null)
    //        {
    //            _dbContext.Profiles.Remove(profile);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //    }
    //}
    public class ProfileDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public ProfileDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Profile>> GetAllProfilesAsync()
        {
            return await _dbContext.Profiles.Include(p => p.ApplicationUser).ToListAsync();
        }

        public async Task<Profile> GetProfileByIdAsync(int userId)
        {
            return await _dbContext.Profiles.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.UserId == userId);
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

        public async Task DeleteProfileAsync(int userId)
        {
            var profile = await GetProfileByIdAsync(userId);
            if (profile != null)
            {
                _dbContext.Profiles.Remove(profile);
                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
