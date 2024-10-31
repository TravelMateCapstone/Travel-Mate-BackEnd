using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IEventParticipantsRepository
    {
        Task<List<EventParticipants>> GetAllEventParticipantsAsync();
        Task<EventParticipants> GetEventParticipantByIdAsync(int eventId, int userId);
        Task<List<EventParticipants>> GetEventParticipantsByEventIdAsync(int eventId); // Thêm phương thức mới
        Task AddEventParticipantAsync(EventParticipants eventParticipant);
        Task RemoveEventParticipantAsync(int eventId, int userId);
        Task<int> GetParticipantCountByEventIdAsync(int eventId);
    }

}
