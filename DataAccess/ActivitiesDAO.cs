using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObjects.Entities;
using BussinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ActivitiesDAO : SingletonBase<ActivitiesDAO>
    {
        private readonly ApplicationDBContext _dbContext;

        public ActivitiesDAO()
        {
            _dbContext = SingletonBase<ActivitiesDAO>._context;
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _dbContext.Activities.ToListAsync();
        }

        public async Task<Activity> GetActivityByIdAsync(int activityId)
        {
            return await _dbContext.Activities.FindAsync(activityId);
        }

        public async Task<Activity> AddActivityAsync(Activity newActivity)
        {
            _dbContext.Activities.Add(newActivity);
            await _dbContext.SaveChangesAsync();
            return newActivity;
        }

        public async Task UpdateActivityAsync(Activity updatedActivity)
        {
            _dbContext.Activities.Update(updatedActivity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteActivityAsync(int activityId)
        {
            var activity = await _dbContext.Activities.FindAsync(activityId);
            if (activity != null)
            {
                _dbContext.Activities.Remove(activity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
