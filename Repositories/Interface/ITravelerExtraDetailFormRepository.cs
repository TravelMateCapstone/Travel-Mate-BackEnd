﻿using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface ITravelerFormRepository
    {
        Task<IEnumerable<TravelerExtraDetailForm>> GetAllAsync();

        Task<IEnumerable<TravelerExtraDetailForm>> GetAllRequests(int userId);
        Task<IEnumerable<TravelerExtraDetailForm>> GetAllChats(int userId);
        Task<TravelerExtraDetailForm> GetByIdAsync(int localId, int travelerId);
        Task<IEnumerable<TravelerExtraDetailForm>> GetByTravelerIdAsync(int travelerId);
        Task AddAsync(TravelerExtraDetailForm form);
        Task ProcessRequest(TravelerExtraDetailForm form);
        Task UpdateAsync(int localId, int travelerId, TravelerExtraDetailForm updatedForm);

        Task DeleteAsync(int localId, int travelerId);

    }
}
