using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ICCCDRepository
    {
        Task<List<CCCD>> GetAllAsync();
        Task<CCCD?> GetByIdAsync(int id);
        Task<CCCD?> GetByIdCCCDAsync(string id);
        Task<CCCD?> GetByUserIdAsync(int userId);
        Task AddAsync(CCCD cccd);
        Task UpdateAsync(CCCD cccd);
        Task DeleteAsync(int id);
    }

}
