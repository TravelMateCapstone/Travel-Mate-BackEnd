using BussinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FindLocalDAO :SingletonBase<FindLocalDAO>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public FindLocalDAO()
        {
        }

        public FindLocalDAO(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetTravelerByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && await _userManager.IsInRoleAsync(user, "traveler") ? user : null;
        }

        public async Task<List<ApplicationUser>> GetLocalsWithMatchingLocationsAsync(List<int> locationIds)
        {
            var localRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "local");
            if (localRole == null) return new List<ApplicationUser>();

            var localUsers = await _context.Users
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == localRole.Id))
                .Where(u => _context.UserLocations.Any(ul => locationIds.Contains(ul.LocationId) && ul.UserId == u.Id.ToString()))
                .ToListAsync();

            return localUsers;
        }

        public async Task<List<int>> GetUserActivityIdsAsync(string userId)
        {
            return await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.ActivityId)
                .ToListAsync();
        }
    }
}
