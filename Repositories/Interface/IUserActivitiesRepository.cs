using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserActivitiesRepository
    {
        Task<List<UserActivity>> GetAllUserActivitiesAsync();
        Task<UserActivity> GetUserActivityByIdAsync(int userId, int activityId);
        Task<UserActivity> AddUserActivityAsync(UserActivity newUserActivity);
        Task UpdateUserActivityAsync(UserActivity updatedUserActivity);
        Task DeleteUserActivityAsync(int userId, int activityId);
    }
}
