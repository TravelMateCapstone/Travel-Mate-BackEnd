using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int locationId);
        Task<Location> AddLocationAsync(Location newLocation);
        Task UpdateLocationAsync(Location updatedLocation);
        Task DeleteLocationAsync(int locationId);
    }

}
