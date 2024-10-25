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
    public class UserHomeRepository : IUserHomeRepository
    {
        private readonly UserHomeDAO _userHomeDAO;

        public UserHomeRepository(UserHomeDAO userHomeDAO)
        {
            _userHomeDAO = userHomeDAO;
        }

        public async Task<List<UserHome>> GetAllUserHomesAsync()
        {
            return await _userHomeDAO.GetAllUserHomesAsync();
        }

        public async Task<UserHome> GetUserHomeByIdAsync(int userId)
        {
            return await _userHomeDAO.GetUserHomeByIdAsync(userId);
        }

        public async Task<UserHome> AddUserHomeAsync(UserHome newUserHome)
        {
            return await _userHomeDAO.AddUserHomeAsync(newUserHome);
        }

        public async Task UpdateUserHomeAsync(UserHome updatedUserHome)
        {
            await _userHomeDAO.UpdateUserHomeAsync(updatedUserHome);
        }

        public async Task DeleteUserHomeAsync(int userId)
        {
            await _userHomeDAO.DeleteUserHomeAsync(userId);
        }
    }

}
