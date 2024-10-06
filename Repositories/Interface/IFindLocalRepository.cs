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
        Task<List<ApplicationUser>> GetMatchingUsers(int travelerId, int locationId);
    }
}
