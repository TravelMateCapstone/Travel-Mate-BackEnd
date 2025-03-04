﻿using BusinessObjects.Entities;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    //public class EventDAO : SingletonBase<EventDAO>
    //{
    //    private readonly ApplicationDBContext _dbContext;

    //    // Constructor, sử dụng SingletonBase để khởi tạo context
    //    public EventDAO()
    //    {
    //        _dbContext = SingletonBase<EventDAO>._context;
    //    }

    //    // Phương thức lấy tất cả các sự kiện
    //    public async Task<List<Event>> GetAllEventsAsync()
    //    {
    //        return await _dbContext.Events.Include(e => e.EventParticipants).ToListAsync();
    //    }

    //    // Phương thức lấy sự kiện theo Id
    //    public async Task<Event> GetEventByIdAsync(int eventId)
    //    {
    //        return await _dbContext.Events.Include(e => e.EventParticipants)
    //                                      .FirstOrDefaultAsync(e => e.EventId == eventId);
    //    }

    //    // Phương thức thêm sự kiện mới
    //    public async Task<Event> AddEventAsync(Event newEvent)
    //    {
    //        _dbContext.Events.Add(newEvent);
    //        await _dbContext.SaveChangesAsync();
    //        return newEvent;
    //    }

    //    // Phương thức cập nhật sự kiện
    //    public async Task UpdateEventAsync(Event updatedEvent)
    //    {
    //        _dbContext.Events.Update(updatedEvent);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    // Phương thức xóa sự kiện
    //    public async Task DeleteEventAsync(int eventId)
    //    {
    //        var eventToDelete = await _dbContext.Events.FindAsync(eventId);
    //        if (eventToDelete != null)
    //        {
    //            _dbContext.Events.Remove(eventToDelete);
    //            await _dbContext.SaveChangesAsync();
    //        }
    //    }
    //}
    public class EventDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public EventDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _dbContext.Events.Include(e => e.EventParticipants)
                                          .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            return await _dbContext.Events.Include(e => e.EventParticipants)
                                          .FirstOrDefaultAsync(e => e.EventId == eventId);
        }

        public async Task<List<Event>> GetEventsByCreaterUserIdAsync(int createrUserId)
        {
            return await _dbContext.Events.Include(e => e.EventParticipants)
                                          .Where(e => e.CreaterUserId == createrUserId)
                                          .ToListAsync();
        }

        public async Task<Event> AddEventAsync(Event newEvent)
        {
            _dbContext.Events.Add(newEvent);
            await _dbContext.SaveChangesAsync();
            return newEvent;
        }

        public async Task UpdateEventAsync(Event updatedEvent)
        {
            _dbContext.Events.Update(updatedEvent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int eventId)
        {
            var existingEvent = await GetEventByIdAsync(eventId);
            if (existingEvent != null)
            {
                _dbContext.Events.Remove(existingEvent);
                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
