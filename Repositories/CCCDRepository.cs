using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CCCDRepository : ICCCDRepository
    {
        private readonly CCCDDAO _dao;

        public CCCDRepository(CCCDDAO dao)
        {
            _dao = dao;
        }

        public async Task<List<CCCD>> GetAllAsync()
        {
            return await _dao.GetAllAsync();
        }

        public async Task<CCCD?> GetByIdAsync(int id)
        {
            return await _dao.GetByIdAsync(id);
        }

        public async Task<CCCD?> GetByIdCCCDAsync(string id)
        {
            return await _dao.GetByIdCCCDAsync(id);
        }
        public async Task<CCCD?> GetByUserIdAsync(int userId)
        {
            return await _dao.GetByUserIdAsync(userId);
        }
        public async Task AddAsync(CCCD cccd)
        {
            await _dao.AddAsync(cccd);
        }

        public async Task UpdateAsync(CCCD cccd)
        {
            await _dao.UpdateAsync(cccd);
        }

        public async Task DeleteAsync(int id)
        {
            await _dao.DeleteAsync(id);
        }
    }

}
