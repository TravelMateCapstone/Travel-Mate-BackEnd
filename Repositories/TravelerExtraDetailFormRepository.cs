﻿using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class TravelerFormRepository : ITravelerFormRepository
    {
        private readonly ExtraFormDetailDAO _travelerFormDAO;

        public TravelerFormRepository(ExtraFormDetailDAO travelerFormDAO)
        {
            _travelerFormDAO = travelerFormDAO;
        }

        public async Task<IEnumerable<TravelerExtraDetailForm>> GetAllAsync()
        {
            return await _travelerFormDAO.GetAllTravelerFormsAsync();
        }

        public async Task<TravelerExtraDetailForm> GetByIdAsync(int localId, int travelerId)
        {
            return await _travelerFormDAO.GetTravelerFormByIdAsync(localId, travelerId);
        }

        public async Task<IEnumerable<TravelerExtraDetailForm>> GetByTravelerIdAsync(int travelerId)
        {
            return await _travelerFormDAO.GetTravelerFormsByTravelerIdAsync(travelerId);
        }

        public async Task AddAsync(TravelerExtraDetailForm form)
        {
            await _travelerFormDAO.AddTravelerFormAsync(form);
        }

        public async Task UpdateAsync(int localId, int travelerId, TravelerExtraDetailForm updatedForm)
        {
            await _travelerFormDAO.UpdateTravelerFormAsync(localId, travelerId, updatedForm);
        }

        public async Task DeleteAsync(int localId, int travelerId)
        {
            await _travelerFormDAO.DeleteTravelerFormAsync(localId, travelerId);
        }
    }
}
