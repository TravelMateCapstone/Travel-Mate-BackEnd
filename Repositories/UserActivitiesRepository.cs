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
    public class UserActivitiesRepository : IUserActivitiesRepository
    {
        private readonly UserActivitiesDAO _userActivitiesDAO;

        public UserActivitiesRepository()
        {
            _userActivitiesDAO = UserActivitiesDAO.Instance;
        }

        public async Task<List<UserActivity>> GetAllUserActivitiesAsync()
        {
            return await _userActivitiesDAO.GetAllUserActivitiesAsync();
        }

        public async Task<UserActivity> GetUserActivityByIdAsync(int userId, int ActivityId)
        {
            return await _userActivitiesDAO.GetUserActivityByIdAsync(userId, ActivityId);
        }

        public async Task<UserActivity> AddUserActivityAsync(UserActivity newUserActivity)
        {
            return await _userActivitiesDAO.AddUserActivityAsync(newUserActivity);
        }

        public async Task UpdateUserActivityAsync(UserActivity updatedUserActivity)
        {
            await _userActivitiesDAO.UpdateUserActivityAsync(updatedUserActivity);
        }

        public async Task DeleteUserActivityAsync(int userId, int ActivityId)
        {
            await _userActivitiesDAO.DeleteUserActivityAsync(userId, ActivityId);
        }
    }
}
