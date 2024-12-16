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
            var allPost = await _pastTripPostDAO.GetAllPostsAsync(userId);

            foreach (var post in allPost)
            {
                var travelerProfile = await _tourRepository.GetUserInfo(post.TravelerId);
                var localProfile = await _tourRepository.GetUserInfo((int)(post.LocalId));

                post.TravelerName = travelerProfile.FullName;
                post.TravelerAvatar = travelerProfile.Profiles.ImageUser;
                post.LocalName = localProfile.FullName;
                post.LocalAvatar = localProfile.Profiles.ImageUser;
                await _pastTripPostDAO.UpdatePostAsync(post.Id, post);
            }

            return allPost;
        }
        public async Task<PastTripPost?> GetPostByIdAsync(string id)
        {
            var existingPost = await _pastTripPostDAO.GetPostByIdAsync(id);

            var travelerProfile = await _tourRepository.GetUserInfo(existingPost.TravelerId);
            var localProfile = await _tourRepository.GetUserInfo((int)(existingPost.LocalId));

            existingPost.TravelerName = travelerProfile.FullName;
            existingPost.TravelerAvatar = travelerProfile.Profiles.ImageUser;
            existingPost.LocalName = localProfile.FullName;
            existingPost.LocalAvatar = localProfile.Profiles.ImageUser;

            await _pastTripPostDAO.UpdatePostAsync(id, existingPost);

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

            foreach (var item in existingTour.Participants)
            {
                if (item.ParticipantId == post.TravelerId)
                {
                    item.PostId = post.Id;
                    break;
                }
            }

            await _tourRepository.UpdateTour(existingTour.TourId, existingTour);
        }

        public async Task<double> GetUserAverageStar(int locaId)
        {
            var listPost = await _pastTripPostDAO.GetAllPostOfUserAsync(locaId);

            double totalStars = 0;
            int postCount = 0;

            foreach (var post in listPost)
            {
                if (post.Star.HasValue)
                {
                    totalStars += post.Star.Value;
                    postCount++;
                }
            }

            if (postCount > 0)
            {
                return totalStars / postCount;
            }

            return 0;
        }

        public async Task<int?> GetUserTotalTrip(int locaId)
        {
            var listPost = await _pastTripPostDAO.GetAllPostOfUserAsync(locaId);

            int postCount = 0;

            foreach (var post in listPost)
            {
                if (post.Star.HasValue)
                {
                    postCount++;
                }
            }
            return postCount;
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
