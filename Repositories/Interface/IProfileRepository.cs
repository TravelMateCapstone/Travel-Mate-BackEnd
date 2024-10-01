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
        Task<IEnumerable<Profile>> GetAllProfilesAsync();
        Task<Profile> GetProfileByIdAsync(string userId);
        Task<Profile> AddProfileAsync(Profile newProfile);
        Task UpdateProfileAsync(Profile updatedProfile);
        Task DeleteProfileAsync(string userId);
    }

}
