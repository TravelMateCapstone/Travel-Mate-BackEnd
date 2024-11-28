using BusinessObjects.Entities;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    //public class LocationsDAO : SingletonBase<LocationsDAO>
    //{
    //    private readonly ApplicationDBContext _dbContext;

    //    public LocationsDAO()
    //    {
    //        _dbContext = SingletonBase<LocationsDAO>._context;
    //    }

    //    public async Task<List<Location>> GetAllLocationsAsync()
    //    {
    //        return await _dbContext.Locations.ToListAsync();
    //    }

    //    public async Task<Location> GetLocationByIdAsync(int locationId)
    //    {
    //        return await _dbContext.Locations.FindAsync(locationId);
    //    }

    //    public async Task<Location> AddLocationAsync(Location newLocation)
    //    {
    //        _dbContext.Locations.Add(newLocation);
    //        await _dbContext.SaveChangesAsync();
    //        return newLocation;
    //    }

    //    public async Task UpdateLocationAsync(Location updatedLocation)
    //    {
    //        _dbContext.Locations.Update(updatedLocation);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    public async Task DeleteLocationAsync(int locationId)
    //    {
    //        var location = await _dbContext.Locations.FindAsync(locationId);
    //        if (location != null)
    //        {
    //            _dbContext.Locations.Remove(location);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //    }


    //    //// Phương thức lấy người dùng theo vai trò "Local", địa điểm và hoạt động
    //    //public async Task<List<ApplicationUser>> GetUsersByLocationAndRoleAsync(int locationId, List<int> activityIds, string roleName)
    //    //{
    //    //    // Tìm role theo tên "Local"
    //    //    // Bước 1: Tìm Role "Local"
    //    //    var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    //    //    if (role == null)
    //    //    {
    //    //        throw new Exception($"Role '{roleName}' not found.");  // Xử lý khi role không tồn tại
    //    //    }

    //    //    // Bước 2: Lọc theo vai trò "Local"
    //    //    var usersWithRole = await _context.UserRoles
    //    //        .Where(ur => ur.RoleId == role.Id)
    //    //        .Select(ur => ur.UserId)
    //    //        .ToListAsync();

    //    //    // Bước 3: Lấy người dùng "Local" có địa điểm trùng và có ít nhất một hoạt động trùng
    //    //    return await _context.Users
    //    //         .Include(u => u.UserLocations)  // Eager Loading cho UserLocations
    //    //         .AsSplitQuery()  // Chia nhỏ truy vấn để tránh lỗi DataReader
    //    //        .Where(u => usersWithRole.Contains(u.Id))  // Chỉ lấy người dùng có role "Local"
    //    //        .Where(u => u.UserLocations != null && u.UserLocations.Any(ul => ul.LocationId == locationId))  // Lọc theo địa điểm
    //    //        .Where(u => u.UserActivities != null && u.UserActivities.Any(ua => activityIds.Contains(ua.ActivityId)))  // Lọc theo hoạt động
    //    //        .ToListAsync();
    //    //}
    //}

    public class LocationsDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public LocationsDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await _dbContext.Locations.ToListAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            return await _dbContext.Locations.FindAsync(locationId);
        }

        //public async Task<Location> AddLocationAsync(Location newLocation)
        //{
        //    _dbContext.Locations.Add(newLocation);
        //    await _dbContext.SaveChangesAsync();
        //    return newLocation;
        //}
        public async Task AddLocationAsync(Location newLocation)
        {
            _dbContext.Locations.AddAsync(newLocation);
            await _dbContext.SaveChangesAsync();
    
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
