using BussinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Entities;

namespace DataAccess
{
    public class UserActivitiesDAO : SingletonBase<UserActivitiesDAO>
    {
        private readonly ApplicationDBContext _dbContext;

        public UserActivitiesDAO()
        {
            _dbContext = SingletonBase<UserActivitiesDAO>._context;
        }

        public async Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync()
        {
            return await _dbContext.UserActivities.Include(ua => ua.Activity).ToListAsync();
        }

        public async Task<UserActivity> GetUserActivityByIdAsync(string userId, int activityId)
        {
            return await _dbContext.UserActivities.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
        }

        public async Task<UserActivity> AddUserActivityAsync(UserActivity newUserActivity)
        {
            _dbContext.UserActivities.Add(newUserActivity);
            await _dbContext.SaveChangesAsync();
            return newUserActivity;
        }

        public async Task UpdateUserActivityAsync(UserActivity updatedUserActivity)
        {
            _dbContext.UserActivities.Update(updatedUserActivity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserActivityAsync(string userId, int activityId)
        {
            var userActivity = await _dbContext.UserActivities.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
            if (userActivity != null)
            {
                _dbContext.UserActivities.Remove(userActivity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
