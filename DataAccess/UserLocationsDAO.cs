﻿using BusinessObjects.Entities;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    //public class UserLocationsDAO : SingletonBase<UserLocationsDAO>
    //{
    //    private readonly ApplicationDBContext _dbContext;

    //    public UserLocationsDAO()
    //    {
    //        _dbContext = SingletonBase<UserLocationsDAO>._context;
    //    }

    //    public async Task<List<UserLocation>> GetAllUserLocationsAsync()
    //    {
    //        return await _dbContext.UserLocations.Include(ul => ul.Location).ToListAsync();
    //    }

    //    public async Task<UserLocation> GetUserLocationByIdAsync(int userId, int locationId)
    //    {
    //        return await _dbContext.UserLocations.FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LocationId == locationId);
    //    }

    //    public async Task<UserLocation> AddUserLocationAsync(UserLocation newUserLocation)
    //    {
    //        _dbContext.UserLocations.Add(newUserLocation);
    //        await _dbContext.SaveChangesAsync();
    //        return newUserLocation;
    //    }

    //    public async Task UpdateUserLocationAsync(UserLocation updatedUserLocation)
    //    {
    //        _dbContext.UserLocations.Update(updatedUserLocation);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    public async Task DeleteUserLocationAsync(int userId, int locationId)
    //    {
    //        var userLocation = await _dbContext.UserLocations.FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LocationId == locationId);
    //        if (userLocation != null)
    //        {
    //            _dbContext.UserLocations.Remove(userLocation);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //    }
    //}

    public class UserLocationsDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public UserLocationsDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserLocation>> GetAllUserLocationsAsync()
        {
            return await _dbContext.UserLocations
                                   .Include(ul => ul.Location)
                                   .Include(ul => ul.ApplicationUser)
                                   .ToListAsync();
        }

        public async Task<UserLocation> GetUserLocationByIdAsync(int userId, int locationId)
        {
            return await _dbContext.UserLocations
                                   .Include(ul => ul.Location)
                                   .Include(ul => ul.ApplicationUser)
                                   .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LocationId == locationId);
        }
        public async Task<List<UserLocation>> GetUserLocationsByUserIdAsync(int userId)
        {
            return await _dbContext.UserLocations.Include(ul => ul.Location)
                                                  .Include(ul => ul.ApplicationUser)
                                                  .Where(ul => ul.UserId == userId)
                                                  .ToListAsync();
        }
        public async Task<UserLocation> AddUserLocationAsync(UserLocation newUserLocation)
        {
            _dbContext.UserLocations.Add(newUserLocation);
            await _dbContext.SaveChangesAsync();
            return newUserLocation;
        }

        public async Task UpdateUserLocationAsync(UserLocation updatedUserLocation)
        {
            _dbContext.UserLocations.Update(updatedUserLocation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserLocationAsync(int userId, int locationId)
        {
            var userLocation = await GetUserLocationByIdAsync(userId, locationId);
            if (userLocation != null)
            {
                _dbContext.UserLocations.Remove(userLocation);
                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
