using BusinessObjects.Entities;
using Microsoft.AspNetCore.Identity;

namespace TravelMateAPI.Services.Role
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        //public async Task<List<string>> GetUserRolesAsync(int userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId.ToString());
        //    if (user == null) throw new Exception("User not found");

        //    var roles = await _userManager.GetRolesAsync(user);
        //    return roles.ToList();
        //}
        public async Task<string> GetUserRoleAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0) throw new Exception("No role assigned to the user");
            if (roles.Count > 1) throw new Exception("User has multiple roles assigned"); // Optional validation

            return roles.FirstOrDefault();
        }

        public async Task UpdateRoleToUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task UpdateRoleToLocalAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, "Local");
        }

        public async Task UpdateRoleToTravelerAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, "Traveler");
        }
    }

}
