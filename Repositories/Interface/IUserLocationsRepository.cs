using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserLocationsRepository
    {
        Task<IEnumerable<UserLocation>> GetAllUserLocationsAsync();
        Task<UserLocation> GetUserLocationByIdAsync(string userId, int locationId);
        Task<UserLocation> AddUserLocationAsync(UserLocation newUserLocation);
        Task UpdateUserLocationAsync(UserLocation updatedUserLocation);
        Task DeleteUserLocationAsync(string userId, int locationId);
    }
}
