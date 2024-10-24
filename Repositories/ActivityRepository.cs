using DataAccess;
using Repositories.Interface;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ActivitiesDAO _activitiesDAO;

        public ActivityRepository(ActivitiesDAO activitiesDAO)
        {
            _activitiesDAO = activitiesDAO;
        }

        public async Task<List<Activity>> GetAllActivitiesAsync()
        {
            return await _activitiesDAO.GetAllActivitiesAsync();
        }

        public async Task<Activity> GetActivityByIdAsync(int activityId)
        {
            return await _activitiesDAO.GetActivityByIdAsync(activityId);
        }

        public async Task<Activity> AddActivityAsync(Activity newActivity)
        {
            return await _activitiesDAO.AddActivityAsync(newActivity);
        }

        public async Task UpdateActivityAsync(Activity updatedActivity)
        {
            await _activitiesDAO.UpdateActivityAsync(updatedActivity);
        }

        public async Task DeleteActivityAsync(int activityId)
        {
            await _activitiesDAO.DeleteActivityAsync(activityId);
        }
    }
}
