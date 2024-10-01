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
    public class UserLocationsRepository : IUserLocationsRepository
    {
        private readonly UserLocationsDAO _userLocationsDAO;

        public UserLocationsRepository()
        {
            _userLocationsDAO = UserLocationsDAO.Instance;
        }

        public async Task<IEnumerable<UserLocation>> GetAllUserLocationsAsync()
        {
            return await _userLocationsDAO.GetAllUserLocationsAsync();
        }

        public async Task<UserLocation> GetUserLocationByIdAsync(string userId, int locationId)
        {
            return await _userLocationsDAO.GetUserLocationByIdAsync(userId, locationId);
        }

        public async Task<UserLocation> AddUserLocationAsync(UserLocation newUserLocation)
        {
            return await _userLocationsDAO.AddUserLocationAsync(newUserLocation);
        }

        public async Task UpdateUserLocationAsync(UserLocation updatedUserLocation)
        {
            await _userLocationsDAO.UpdateUserLocationAsync(updatedUserLocation);
        }

        public async Task DeleteUserLocationAsync(string userId, int locationId)
        {
            await _userLocationsDAO.DeleteUserLocationAsync(userId, locationId);
        }
    }
}
