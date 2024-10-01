using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserActivitiesRepository
    {
        Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync();
        Task<UserActivity> GetUserActivityByIdAsync(string userId, int activityId);
        Task<UserActivity> AddUserActivityAsync(UserActivity newUserActivity);
        Task UpdateUserActivityAsync(UserActivity updatedUserActivity);
        Task DeleteUserActivityAsync(string userId, int activityId);
    }
}
