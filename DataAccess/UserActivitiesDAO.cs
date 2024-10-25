using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace DataAccess
{
    //public class UserActivitiesDAO : SingletonBase<UserActivitiesDAO>
    //{
    //    private readonly ApplicationDBContext _dbContext;

    //    public UserActivitiesDAO()
    //    {
    //        _dbContext = SingletonBase<UserActivitiesDAO>._context;
    //    }

    //    public async Task<List<UserActivity>> GetAllUserActivitiesAsync()
    //    {
    //        return await _dbContext.UserActivities.Include(ua => ua.Activity).ToListAsync();
    //    }

    //    public async Task<UserActivity> GetUserActivityByIdAsync(int userId, int activityId)
    //    {
    //        return await _dbContext.UserActivities.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
    //    }

    //    public async Task<UserActivity> AddUserActivityAsync(UserActivity newUserActivity)
    //    {
    //        _dbContext.UserActivities.Add(newUserActivity);
    //        await _dbContext.SaveChangesAsync();
    //        return newUserActivity;
    //    }

    //    public async Task UpdateUserActivityAsync(UserActivity updatedUserActivity)
    //    {
    //        _dbContext.UserActivities.Update(updatedUserActivity);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    public async Task DeleteUserActivityAsync(int userId, int activityId)
    //    {
    //        var userActivity = await _dbContext.UserActivities.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
    //        if (userActivity != null)
    //        {
    //            _dbContext.UserActivities.Remove(userActivity);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //    }
    //}

    public class UserActivitiesDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public UserActivitiesDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserActivity>> GetAllUserActivitiesAsync()
        {
            return await _dbContext.UserActivities
                                   .Include(ua => ua.Activity)
                                   .Include(ua => ua.ApplicationUser)
                                   .ToListAsync();
        }

        public async Task<UserActivity> GetUserActivityByIdAsync(int userId, int activityId)
        {
            return await _dbContext.UserActivities
                                   .Include(ua => ua.Activity)
                                   .Include(ua => ua.ApplicationUser)
                                   .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
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

        public async Task DeleteUserActivityAsync(int userId, int activityId)
        {
            var userActivity = await GetUserActivityByIdAsync(userId, activityId);
            if (userActivity != null)
            {
                _dbContext.UserActivities.Remove(userActivity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
