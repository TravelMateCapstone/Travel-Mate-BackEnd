using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserHomeRepository
    {
        Task<List<UserHome>> GetAllUserHomesAsync();
        Task<UserHome> GetUserHomeByIdAsync(int userId);
        Task<UserHome> AddUserHomeAsync(UserHome newUserHome);
        Task UpdateUserHomeAsync(UserHome updatedUserHome);
        Task DeleteUserHomeAsync(int userId);
    }

}
