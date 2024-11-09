using BusinessObjects.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories
{
    public class EventParticipantsRepository : IEventParticipantsRepository
    {
        private readonly EventParticipantsDAO _eventParticipantsDAO;

        public EventParticipantsRepository(EventParticipantsDAO eventParticipantsDAO)
        {
            _eventParticipantsDAO = eventParticipantsDAO;
        }

        public async Task<List<EventParticipants>> GetAllEventParticipantsAsync()
        {
            return await _eventParticipantsDAO.GetAllEventParticipantsAsync();
        }

        public async Task<EventParticipants> GetEventParticipantByIdAsync(int eventId, int userId)
        {
            return await _eventParticipantsDAO.GetEventParticipantByIdAsync(eventId, userId);
        }

        public async Task<List<EventParticipants>> GetEventParticipantsByEventIdAsync(int eventId)
        {
            return await _eventParticipantsDAO.GetEventParticipantsByEventIdAsync(eventId);
        }

        public async Task AddEventParticipantAsync(EventParticipants eventParticipant)
        {
            await _eventParticipantsDAO.AddEventParticipantAsync(eventParticipant);
        }

        public async Task RemoveEventParticipantAsync(int eventId, int userId)
        {
            await _eventParticipantsDAO.RemoveEventParticipantAsync(eventId, userId);
        }
        // Đếm số lượng người tham gia cho một sự kiện
        public async Task<int> GetParticipantCountByEventIdAsync(int eventId)
        {
            return await _eventParticipantsDAO.GetParticipantCountByEventIdAsync(eventId);
        }
        public async Task<bool> HasUserJoinedEventAsync(int eventId, int userId)
        {
            return await _eventParticipantsDAO.HasUserJoinedEventAsync(eventId, userId);
        }

    }


}
