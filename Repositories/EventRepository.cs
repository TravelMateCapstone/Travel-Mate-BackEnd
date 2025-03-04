﻿using BusinessObjects.Entities;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDAO _eventDAO;

        public EventRepository(EventDAO eventDAO)
        {
            _eventDAO = eventDAO;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _eventDAO.GetAllEventsAsync();
        }

        public async Task<Event> GetEventByIdAsync(int eventId)
        {
            return await _eventDAO.GetEventByIdAsync(eventId);
        }

        public async Task<List<Event>> GetEventsByCreaterUserIdAsync(int createrUserId)
        {
            return await _eventDAO.GetEventsByCreaterUserIdAsync(createrUserId);
        }

        public async Task<Event> AddEventAsync(Event newEvent)
        {
            return await _eventDAO.AddEventAsync(newEvent);
        }

        public async Task UpdateEventAsync(Event updatedEvent)
        {
            await _eventDAO.UpdateEventAsync(updatedEvent);
        }

        public async Task DeleteEventAsync(int eventId)
        {
            await _eventDAO.DeleteEventAsync(eventId);
        }
    }


}
