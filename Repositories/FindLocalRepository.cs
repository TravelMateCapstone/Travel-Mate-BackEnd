using BussinessObjects.Entities;
using BussinessObjects;
using Microsoft.AspNetCore.Identity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Repositories
{
    public class FindLocalRepository : IFindLocalRepository
    {
        private readonly FindLocalDAO _findLocalDAO;

        public FindLocalRepository(FindLocalDAO findLocalDAO)
        {
            _findLocalDAO = findLocalDAO;
        }

        public Task<ApplicationUser> GetTravelerByIdAsync(string userId) =>
            _findLocalDAO.GetTravelerByIdAsync(userId);

        public Task<List<ApplicationUser>> GetLocalsWithMatchingLocationsAsync(List<int> locationIds) =>
            _findLocalDAO.GetLocalsWithMatchingLocationsAsync(locationIds);

        public Task<List<int>> GetUserActivityIdsAsync(string userId) =>
            _findLocalDAO.GetUserActivityIdsAsync(userId);
    }
}
