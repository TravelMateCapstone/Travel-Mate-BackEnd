using BussinessObjects.Entities;
using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ProfileDAO _profileDAO;

        public UserProfileRepository()
        {
            _profileDAO = ProfileDAO.Instance;
        }

        public async Task<List<UserProfile>> GetAllProfilesAsync()
        {
            return await _profileDAO.GetAllProfilesAsync();
        }

        public async Task<UserProfile> GetProfileByIdAsync(int userId)
        {
            return await _profileDAO.GetProfileByIdAsync(userId);
        }

        public async Task<UserProfile> AddProfileAsync(UserProfile newProfile)
        {
            return await _profileDAO.AddProfileAsync(newProfile);
        }

        public async Task UpdateProfileAsync(UserProfile updatedProfile)
        {
            await _profileDAO.UpdateProfileAsync(updatedProfile);
        }

        public async Task DeleteProfileAsync(int userId)
        {
            await _profileDAO.DeleteProfileAsync(userId);
        }
    }

}
