using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IProfileRepository
    {
        Task<List<Profile>> GetAllProfilesAsync();
        Task<Profile> GetProfileByIdAsync(int userId);
        Task<Profile> AddProfileAsync(Profile newProfile);
        Task UpdateProfileAsync(Profile updatedProfile);
        Task DeleteProfileAsync(int userId);
    }

}
