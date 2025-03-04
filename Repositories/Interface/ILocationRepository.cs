﻿using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int locationId);
        Task AddLocationAsync(Location newLocation);
        Task UpdateLocationAsync(Location updatedLocation);
        Task DeleteLocationAsync(int locationId);
    }


}
