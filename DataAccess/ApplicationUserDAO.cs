using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ApplicationUserDAO 
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserDAO(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //// Lấy tất cả người dùng
        //public async Task<List<ApplicationUser>> GetAllUsersAsync()
        //{
        //    return await _context.Users.Include(e => e.Profiles).Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
        //    //return await _dbContext.Events.Include(e => e.EventParticipants).ToListAsync();
        //}
        //public async Task<IQueryable<ApplicationUser>> GetAllUsersAsync()
        //{
        //    return _context.Users.Include(u => u.Profiles);
        //}
        // Lấy tất cả người dùng cùng với vai trò của họ

        //public async Task<List<(ApplicationUser User, List<string> Roles)>> GetAllUsersAsync()
        //{
        //    var users = await _context.Users.Include(u => u.Profiles).ToListAsync();
        //    var usersWithRoles = new List<(ApplicationUser, List<string>)>();

        //    foreach (var user in users)
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);
        //        usersWithRoles.Add((user, roles.ToList()));
        //    }

        //    return usersWithRoles;
        //}
        public async Task<List<ApplicationUserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Profiles).ToListAsync();
            var result = new List<ApplicationUserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new ApplicationUserDTO
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Profile = user.Profiles == null ? null : new ProfileDTO
                    {
                        ProfileId = user.Profiles.ProfileId,
                        Address = user.Profiles.Address
                    }
                });
            }

            return result;
        }

        // Tìm người dùng theo ID
        public async Task<ApplicationUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Tìm người dùng bằng email
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Thêm user mới
        public async Task AddUserAsync(ApplicationUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin user
        public void UpdateUser(ApplicationUser user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // Xóa user
        public void DeleteUser(ApplicationUser user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
