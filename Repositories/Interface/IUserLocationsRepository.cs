using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserLocationsRepository
    {
        Task<List<UserLocation>> GetAllUserLocationsAsync();
        Task<UserLocation> GetUserLocationByIdAsync(int userId, int locationId);
        Task<UserLocation> AddUserLocationAsync(UserLocation newUserLocation);
        Task UpdateUserLocationAsync(UserLocation updatedUserLocation);
        Task DeleteUserLocationAsync(int userId, int locationId);
    }
}
