using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserEducationRepository
    {
        Task<List<UserEducation>> GetAllUserEducationsAsync();
        Task<UserEducation> GetUserEducationByIdAsync(int universityId, int userId);
        Task<UserEducation> AddUserEducationAsync(UserEducation newUserEducation);
        Task UpdateUserEducationAsync(UserEducation updatedUserEducation);
        Task DeleteUserEducationAsync(int universityId, int userId);
    }

}
