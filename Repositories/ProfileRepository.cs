using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileDAO _profileDAO;

        public ProfileRepository(ProfileDAO profileDAO)
        {
            _profileDAO = profileDAO;
        }

        public async Task<List<Profile>> GetAllProfilesAsync()
        {
            return await _profileDAO.GetAllProfilesAsync();
        }

        public async Task<Profile> GetProfileByIdAsync(int userId)
        {
            return await _profileDAO.GetProfileByIdAsync(userId);
        }

        public async Task<Profile> AddProfileAsync(Profile newProfile)
        {
            return await _profileDAO.AddProfileAsync(newProfile);
        }

        public async Task UpdateProfileAsync(Profile updatedProfile)
        {
            await _profileDAO.UpdateProfileAsync(updatedProfile);
        }

        public async Task DeleteProfileAsync(int userId)
        {
            await _profileDAO.DeleteProfileAsync(userId);
        }
    }

}
