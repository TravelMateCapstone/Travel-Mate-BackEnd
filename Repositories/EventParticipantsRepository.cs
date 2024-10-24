using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class EventParticipantsRepository : IEventParticipantsRepository
    {
        private readonly EventParticipantsDAO _eventParticipantsDAO;

        public EventParticipantsRepository()
        {
            _eventParticipantsDAO = EventParticipantsDAO.Instance;
        }

        public async Task<List<EventParticipants>> GetAllParticipantsAsync()
        {
            return await _eventParticipantsDAO.GetAllParticipantsAsync();
        }

        public async Task<EventParticipants> GetParticipantByIdAsync(int participantId)
        {
            return await _eventParticipantsDAO.GetParticipantByIdAsync(participantId);
        }

        public async Task<EventParticipants> AddParticipantAsync(EventParticipants newParticipant)
        {
            return await _eventParticipantsDAO.AddParticipantAsync(newParticipant);
        }

        public async Task UpdateParticipantAsync(EventParticipants updatedParticipant)
        {
            await _eventParticipantsDAO.UpdateParticipantAsync(updatedParticipant);
        }

        public async Task DeleteParticipantAsync(int participantId)
        {
            await _eventParticipantsDAO.DeleteParticipantAsync(participantId);
        }
    }

}
