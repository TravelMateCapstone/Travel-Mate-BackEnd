using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IFindLocalRepository
    {
        Task<ApplicationUser> GetTravelerByIdAsync(string userId);
        Task<List<ApplicationUser>> GetLocalsWithMatchingLocationsAsync(List<int> locationIds);
        Task<List<int>> GetUserActivityIdsAsync(string userId);
    }
}
