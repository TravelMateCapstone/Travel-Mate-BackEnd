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
    public class LocationRepository : ILocationRepository
    {
        private readonly LocationsDAO _locationsDAO;

        public LocationRepository()
        {
            _locationsDAO = LocationsDAO.Instance;
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await _locationsDAO.GetAllLocationsAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            return await _locationsDAO.GetLocationByIdAsync(locationId);
        }

        public async Task<Location> AddLocationAsync(Location newLocation)
        {
            return await _locationsDAO.AddLocationAsync(newLocation);
        }

        public async Task UpdateLocationAsync(Location updatedLocation)
        {
            await _locationsDAO.UpdateLocationAsync(updatedLocation);
        }

        public async Task DeleteLocationAsync(int locationId)
        {
            await _locationsDAO.DeleteLocationAsync(locationId);
        }
    }

}
