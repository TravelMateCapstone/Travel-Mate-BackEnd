using BussinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserEducationRepository : IUserEducationRepository
    {
        private readonly UserEducationDAO _userEducationDAO;

        public UserEducationRepository(UserEducationDAO userEducationDAO)
        {
            _userEducationDAO = userEducationDAO;
        }

        public async Task<List<UserEducation>> GetAllUserEducationsAsync()
        {
            return await _userEducationDAO.GetAllUserEducationsAsync();
        }

        public async Task<UserEducation> GetUserEducationByIdAsync(int universityId, int userId)
        {
            return await _userEducationDAO.GetUserEducationByIdAsync(universityId, userId);
        }

        public async Task<UserEducation> AddUserEducationAsync(UserEducation newUserEducation)
        {
            return await _userEducationDAO.AddUserEducationAsync(newUserEducation);
        }

        public async Task UpdateUserEducationAsync(UserEducation updatedUserEducation)
        {
            await _userEducationDAO.UpdateUserEducationAsync(updatedUserEducation);
        }

        public async Task DeleteUserEducationAsync(int universityId, int userId)
        {
            await _userEducationDAO.DeleteUserEducationAsync(universityId, userId);
        }
    }

}
