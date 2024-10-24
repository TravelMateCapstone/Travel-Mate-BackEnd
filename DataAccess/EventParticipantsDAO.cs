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
    public class EventParticipantsDAO : SingletonBase<EventParticipantsDAO>
    {
        private readonly ApplicationDBContext _dbContext;

        // Constructor
        public EventParticipantsDAO()
        {
            _dbContext = SingletonBase<EventParticipantsDAO>._context;
        }

        // Phương thức lấy tất cả những người tham gia sự kiện
        public async Task<List<EventParticipants>> GetAllParticipantsAsync()
        {
            return await _dbContext.EventParticipants.Include(ep => ep.Event).ToListAsync();
        }

        // Phương thức lấy người tham gia theo Id
        public async Task<EventParticipants> GetParticipantByIdAsync(int participantId)
        {
            return await _dbContext.EventParticipants.Include(ep => ep.Event)
                                                     .FirstOrDefaultAsync(ep => ep.EventParticipantId == participantId);
        }

        // Phương thức thêm người tham gia sự kiện mới
        public async Task<EventParticipants> AddParticipantAsync(EventParticipants newParticipant)
        {
            _dbContext.EventParticipants.Add(newParticipant);
            await _dbContext.SaveChangesAsync();
            return newParticipant;
        }

        // Phương thức cập nhật người tham gia
        public async Task UpdateParticipantAsync(EventParticipants updatedParticipant)
        {
            _dbContext.EventParticipants.Update(updatedParticipant);
            await _dbContext.SaveChangesAsync();
        }

        // Phương thức xóa người tham gia sự kiện
        public async Task DeleteParticipantAsync(int participantId)
        {
            var participantToDelete = await _dbContext.EventParticipants.FindAsync(participantId);
            if (participantToDelete != null)
            {
                _dbContext.EventParticipants.Remove(participantToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
