using BusinessObjects.Entities;
using BusinessObjects.Utils.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IApplicationUserRepository
    {
        //Task<List<ApplicationUser>> GetAllUsersAsync();
        //Task<List<(ApplicationUser User, List<string> Roles)>> GetAllUsersAsync();
        Task<List<ApplicationUserDTO>> GetAllUsersAsync();
        //Task<IQueryable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(int id);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task AddUserAsync(ApplicationUser user);
        void UpdateUser(ApplicationUser user);
        void DeleteUser(ApplicationUser user);
    }
}
