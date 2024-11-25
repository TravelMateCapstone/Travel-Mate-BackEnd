using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationUserDAO _userDAO;


        public ApplicationUserRepository(ApplicationUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userDAO.GetAllUsersAsync();
        }
        //public async Task<IQueryable<ApplicationUser>> GetAllUsersAsync()
        //{
        //    return await _userDAO.GetAllUsersAsync();
        //}


        public async Task<ApplicationUser> GetUserByIdAsync(int id)
        {
            return await _userDAO.GetUserByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userDAO.GetUserByEmailAsync(email);
        }

        public async Task AddUserAsync(ApplicationUser user)
        {
            await _userDAO.AddUserAsync(user);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _userDAO.UpdateUser(user);
        }

        public void DeleteUser(ApplicationUser user)
        {
            _userDAO.DeleteUser(user);
        }
    }
}
