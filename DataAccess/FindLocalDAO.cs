using BussinessObjects;
using BussinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace DataAccess
{
    //public class FindLocalDAO :SingletonBase<FindLocalDAO>
    //{
    //    private readonly UserManager<ApplicationUser> _userManager;

    //    public FindLocalDAO()
    //    {
    //    }

    //    public FindLocalDAO(UserManager<ApplicationUser> userManager)
    //    {
    //        _userManager = userManager;
    //    }

    //    public async Task<ApplicationUser> GetTravelerByIdAsync(int userId)
    //    {
    //        var user = await _userManager.FindByIdAsync(userId);
    //        return user != null && await _userManager.IsInRoleAsync(user, "traveler") ? user : null;
    //    }

    //    public async Task<List<ApplicationUser>> GetLocalsWithMatchingLocationsAsync(List<int> locationIds)
    //    {
    //        var localRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "local");
    //        if (localRole == null) return new List<ApplicationUser>();

    //        var localUsers = await _context.Users
    //            .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == localRole.Id))
    //            .Where(u => _context.UserLocations.Any(ul => locationIds.Contains(ul.LocationId) && ul.UserId == u.Id))
    //            .ToListAsync();

    //        return localUsers;
    //    }

    //    public async Task<List<int>> GetUserActivityIdsAsync(int userId)
    //    {
    //        return await _context.UserActivities
    //            .Where(ua => ua.UserId == userId)
    //            .Select(ua => ua.ActivityId)
    //            .ToListAsync();
    //    }
    //}
    public class FindLocalDAO : SingletonBase<FindLocalDAO>
    {
        //private readonly UserManager<ApplicationUser> _userManager;

        public FindLocalDAO()
        {
        }
        private readonly string _connectionString;

        public FindLocalDAO(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        public async Task<List<int>> GetTravelerActivitiesAsync(int travelerId)
        {
            return await _context.UserActivities
                .Where(ua => ua.UserId == travelerId)
                .Select(ua => ua.ActivityId)
                .ToListAsync();
        }
        public async Task<List<ApplicationUser>> GetUsersByLocationAndRoleAsync(int locationId, List<int> activityIds, string roleName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Tạo câu truy vấn SQL tương ứng
                var sql = @"
                SELECT u.Id, u.FullName, u.Email, u.RegistrationTime, COUNT(ua.ActivityId) AS MatchingActivitiesCount
                FROM Users u
                INNER JOIN UserRoles ur ON u.Id = ur.UserId
                INNER JOIN Roles r ON ur.RoleId = r.Id
                INNER JOIN UserLocations ul ON u.Id = ul.UserId
                INNER JOIN UserActivities ua ON u.Id = ua.UserId
                WHERE r.Name = @RoleName
                  AND ul.LocationId = @LocationId
                  AND ua.ActivityId IN @ActivityIds
                GROUP BY u.Id, u.FullName, u.Email, u.RegistrationTime
                ORDER BY COUNT(ua.ActivityId) DESC;";  // Sắp xếp theo số lượng hoạt động khớp

                // Sử dụng Dapper để thực hiện truy vấn
                var users = await connection.QueryAsync<ApplicationUser, int, ApplicationUser>(
                    sql,
                    (user, matchingActivitiesCount) =>
                    {
                        user.MatchingActivitiesCount = matchingActivitiesCount;
                        return user;
                    },
                    new
                    {
                        RoleName = roleName,
                        LocationId = locationId,
                        ActivityIds = activityIds  // Dapper sẽ tự động xử lý danh sách cho lệnh IN
                    },
                    splitOn: "MatchingActivitiesCount"
                );

                return users.ToList();
            }
        }

    }

}
