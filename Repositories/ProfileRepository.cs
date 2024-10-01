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
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileDAO _profileDAO;

        public ProfileRepository()
        {
            _profileDAO = ProfileDAO.Instance;
        }

        public async Task<IEnumerable<Profile>> GetAllProfilesAsync()
        {
            return await _profileDAO.GetAllProfilesAsync();
        }

        public async Task<Profile> GetProfileByIdAsync(string userId)
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

        public async Task DeleteProfileAsync(string userId)
        {
            await _profileDAO.DeleteProfileAsync(userId);
        }
    }

}
