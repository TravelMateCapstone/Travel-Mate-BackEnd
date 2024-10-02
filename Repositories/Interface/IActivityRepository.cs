using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Entities;

namespace Repositories.Interface
{
    public interface IActivityRepository
    {
        Task<List<Activity>> GetAllActivitiesAsync();
        Task<Activity> GetActivityByIdAsync(int activityId);
        Task<Activity> AddActivityAsync(Activity newActivity);
        Task UpdateActivityAsync(Activity updatedActivity);
        Task DeleteActivityAsync(int activityId);
    }
}
