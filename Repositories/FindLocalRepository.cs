using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class FindLocalRepository : IFindLocalRepository
    {
        //private readonly FindLocalDAO _findLocalDAO;

        //public FindLocalRepository(FindLocalDAO findLocalDAO)
        //{
        //    _findLocalDAO = findLocalDAO;
        //}

        //public Task<ApplicationUser> GetTravelerByIdAsync(int userId) =>
        //    _findLocalDAO.GetTravelerByIdAsync(userId);

        //public Task<List<ApplicationUser>> GetLocalsWithMatchingLocationsAsync(List<int> locationIds) =>
        //    _findLocalDAO.GetLocalsWithMatchingLocationsAsync(locationIds);

        //public Task<List<int>> GetUserActivityIdsAsync(int userId) =>
        //    _findLocalDAO.GetUserActivityIdsAsync(userId);

        //private readonly ApplicationDBContext _context;

        //public FindLocalRepository(ApplicationDBContext context)
        //{
        //    _context = context;
        //}
        private readonly FindLocalDAO _findLocalDAO;

        public FindLocalRepository(FindLocalDAO findLocalDAO)
        {
            _findLocalDAO = findLocalDAO;
            
        }

        public async Task<List<ApplicationUser>> GetMatchingUsers(int travelerId, int locationId)
        {
            // Lấy danh sách các hoạt động của traveler
            var travelerActivities = await _findLocalDAO.GetTravelerActivitiesAsync(travelerId);

            //// Tìm tất cả người dùng có vai trò "Local"
            //var localUsers = await _findLocalDAO.GetUsersByRoleAsync("Local");

            //// Lọc những người dùng "Local" theo địa điểm và sở thích hoạt động
            //var matchingLocalUsers = localUsers
            //    .Where(u => u.UserLocations.Any(ul => ul.LocationId == locationId) &&
            //                u.UserActivities.Any(ua => travelerActivities.Contains(ua.ActivityId)))
            //    .OrderByDescending(u => u.UserActivities.Count(ua => travelerActivities.Contains(ua.ActivityId)))
            //    .ToList();



            //// Lấy danh sách những người dùng local có địa điểm trùng khớp
            //var matchingLocalUsers = localUsers
            //.Where(u => u.UserLocations != null && u.UserLocations.Any(ul => ul.LocationId == locationId)) // Kiểm tra UserLocations không null trước khi lọc
            //.Select(u => new
            //{
            //    User = u,
            //    // Tìm các hoạt động chung giữa user local và traveler
            //    CommonActivities = u.UserActivities != null
            //        ? u.UserActivities
            //            .Select(ua => ua.ActivityId)
            //            .Intersect(travelerActivities) // Tìm các hoạt động chung
            //            .ToList()
            //        : new List<int>() // Nếu UserActivities là null, trả về danh sách rỗng
            //})
            //.Where(u => u.CommonActivities.Any()) // Chỉ giữ lại người dùng có ít nhất một hoạt động chung
            //.OrderByDescending(u => u.CommonActivities.Count) // Sắp xếp theo số lượng hoạt động chung
            //.Select(u => u.User) // Trả về đối tượng User sau khi đã lọc và sắp xếp
            //.ToList();


            // Kiểm tra để đảm bảo travelerActivities không null hoặc rỗng
            if (travelerActivities == null || !travelerActivities.Any())
            {
                return new List<ApplicationUser>(); // Trả về danh sách rỗng nếu không có hoạt động nào
            }

            // Tìm tất cả người dùng local có cùng địa điểm và ít nhất một hoạt động chung với traveler
            var matchingLocalUsers = await _findLocalDAO.GetUsersByLocationAndRoleAsync(locationId, travelerActivities, "Local");

            // Trả về danh sách người dùng local đã được tìm thấy
            return matchingLocalUsers;
        }
    }
}
