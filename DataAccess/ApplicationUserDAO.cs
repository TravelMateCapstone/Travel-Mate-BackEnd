using BussinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ApplicationUserDAO : SingletonBase<ApplicationUserDAO>
    {
        // Lấy tất cả người dùng
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
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
