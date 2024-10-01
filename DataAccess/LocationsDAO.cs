using BussinessObjects.Entities;
using BussinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class LocationsDAO : SingletonBase<LocationsDAO>
    {
        private readonly ApplicationDBContext _dbContext;

        public LocationsDAO()
        {
            _dbContext = SingletonBase<LocationsDAO>._context;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _dbContext.Locations.ToListAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            return await _dbContext.Locations.FindAsync(locationId);
        }

        public async Task<Location> AddLocationAsync(Location newLocation)
        {
            _dbContext.Locations.Add(newLocation);
            await _dbContext.SaveChangesAsync();
            return newLocation;
        }

        public async Task UpdateLocationAsync(Location updatedLocation)
        {
            _dbContext.Locations.Update(updatedLocation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int locationId)
        {
            var location = await _dbContext.Locations.FindAsync(locationId);
            if (location != null)
            {
                _dbContext.Locations.Remove(location);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
