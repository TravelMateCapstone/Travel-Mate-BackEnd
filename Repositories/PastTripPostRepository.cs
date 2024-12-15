using BusinessObjects;
using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using Repository.Interfaces;

namespace Repositories
{
    public class PastTripPostRepository : IPastTripPostRepository
    {
        private readonly PastTripPostDAO _pastTripPostDAO;
        private readonly ITourRepository _tourRepository;

        public PastTripPostRepository(PastTripPostDAO pastTripPostDAO, ITourRepository tourRepository)
        {
            _pastTripPostDAO = pastTripPostDAO;
            _tourRepository = tourRepository;
        }
        public async Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId)
        {
            return await _pastTripPostDAO.GetAllPostsAsync(userId);
        }
        public async Task<PastTripPost?> GetPostByIdAsync(string id)
        {
            return await _pastTripPostDAO.GetPostByIdAsync(id);
        }

        public async Task AddAsync(PastTripPost post)
        {
            var existingTour = await _tourRepository.GetTourById(post.TourId);
            post.LocalId = existingTour.Creator.Id;
            post.Location = existingTour.Location;
            post.CreatedAt = GetTimeZone.GetVNTimeZoneNow();
            post.IsCaptionEdit = false;

            await _pastTripPostDAO.AddPostAsync(post);
        }

        public async Task DeleteAsync(string postId)
        {
            await _pastTripPostDAO.DeletePostAsync(postId);
        }

        public async Task UpdatePostAsync(string postId, PastTripPost post)
        {
            await _pastTripPostDAO.UpdatePostAsync(postId, post);
        }
    }
}
