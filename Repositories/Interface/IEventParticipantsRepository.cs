using BussinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IEventParticipantsRepository
    {
        Task<IEnumerable<EventParticipants>> GetAllParticipantsAsync();
        Task<EventParticipants> GetParticipantByIdAsync(int participantId);
        Task<EventParticipants> AddParticipantAsync(EventParticipants newParticipant);
        Task UpdateParticipantAsync(EventParticipants updatedParticipant);
        Task DeleteParticipantAsync(int participantId);
    }
}
