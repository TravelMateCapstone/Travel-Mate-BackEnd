using BusinessObjects.Entities;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CCCDDAO
    {
        private readonly ApplicationDBContext _context;

        public CCCDDAO(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<CCCD>> GetAllAsync()
        {
            return await _context.CCCDs.Include(c => c.User).ToListAsync();
        }

        public async Task<CCCD?> GetByIdAsync(int id)
        {
            return await _context.CCCDs.Include(c => c.User).FirstOrDefaultAsync(c => c.CCCDId == id);
        }

        public async Task<CCCD?> GetByIdCCCDAsync(string id)
        {
            return await _context.CCCDs.Include(c => c.User).FirstOrDefaultAsync(c => c.id == id);
        }

        // Tìm CCCD theo UserId
        public async Task<CCCD?> GetByUserIdAsync(int userId)
        {
            return await _context.CCCDs
                                 .Where(c => c.UserId == userId)
                                 .FirstOrDefaultAsync();
        }

        public async Task AddAsync(CCCD cccd)
        {
            await _context.CCCDs.AddAsync(cccd);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CCCD cccd)
        {
            _context.CCCDs.Update(cccd);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cccd = await GetByIdAsync(id);
            if (cccd != null)
            {
                _context.CCCDs.Remove(cccd);
                await _context.SaveChangesAsync();
            }
        }
    }

}
