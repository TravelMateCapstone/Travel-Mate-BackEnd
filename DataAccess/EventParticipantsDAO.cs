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
    //public class EventParticipantsDAO : SingletonBase<EventParticipantsDAO>
    //{
    //    private readonly ApplicationDBContext _dbContext;

    //    // Constructor
    //    public EventParticipantsDAO()
    //    {
    //        _dbContext = SingletonBase<EventParticipantsDAO>._context;
    //    }

    //    // Phương thức lấy tất cả những người tham gia sự kiện
    //    public async Task<List<EventParticipants>> GetAllParticipantsAsync()
    //    {
    //        return await _dbContext.EventParticipants.Include(ep => ep.Event).ToListAsync();
    //    }

    //    // Phương thức lấy người tham gia theo Id
    //    public async Task<EventParticipants> GetParticipantByIdAsync(int participantId)
    //    {
    //        return await _dbContext.EventParticipants.Include(ep => ep.Event)
    //                                                 .FirstOrDefaultAsync(ep => ep.EventParticipantId == participantId);
    //    }

    //    // Phương thức thêm người tham gia sự kiện mới
    //    public async Task<EventParticipants> AddParticipantAsync(EventParticipants newParticipant)
    //    {
    //        _dbContext.EventParticipants.Add(newParticipant);
    //        await _dbContext.SaveChangesAsync();
    //        return newParticipant;
    //    }

    //    // Phương thức cập nhật người tham gia
    //    public async Task UpdateParticipantAsync(EventParticipants updatedParticipant)
    //    {
    //        _dbContext.EventParticipants.Update(updatedParticipant);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    // Phương thức xóa người tham gia sự kiện
    //    public async Task DeleteParticipantAsync(int participantId)
    //    {
    //        var participantToDelete = await _dbContext.EventParticipants.FindAsync(participantId);
    //        if (participantToDelete != null)
    //        {
    //            _dbContext.EventParticipants.Remove(participantToDelete);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //    }
    //}

    public class EventParticipantsDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public EventParticipantsDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EventParticipants>> GetAllEventParticipantsAsync()
        {
            return await _dbContext.EventParticipants.Include(ep => ep.Event)
                                                     .Include(ep => ep.ApplicationUser)
                                                     .ToListAsync();
        }

        public async Task<EventParticipants> GetEventParticipantByIdAsync(int eventId, int userId)
        {
            return await _dbContext.EventParticipants.Include(ep => ep.Event)
                                                     .Include(ep => ep.ApplicationUser)
                                                     .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);
        }

        public async Task<List<EventParticipants>> GetEventParticipantsByEventIdAsync(int eventId)
        {
            return await _dbContext.EventParticipants.Include(ep => ep.Event)
                                                     .Include(ep => ep.ApplicationUser)
                                                     .Where(ep => ep.EventId == eventId)
                                                     .ToListAsync();
        }

        public async Task AddEventParticipantAsync(EventParticipants eventParticipant)
        {
            _dbContext.EventParticipants.Add(eventParticipant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveEventParticipantAsync(int eventId, int userId)
        {
            var participant = await GetEventParticipantByIdAsync(eventId, userId);
            if (participant != null)
            {
                _dbContext.EventParticipants.Remove(participant);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<int> GetParticipantCountByEventIdAsync(int eventId)
        {
            return await _dbContext.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .CountAsync();
        }
    }


}
