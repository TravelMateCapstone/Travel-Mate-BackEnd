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
        //public async Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId)
        //{
        //    var allPost = await _pastTripPostDAO.GetAllPostsAsync(userId);

        //    foreach (var post in allPost)
        //    {
        //        var travelerProfile = await _tourRepository.GetUserInfo(post.TravelerId);
        //        var localProfile = await _tourRepository.GetUserInfo((int)(post.LocalId));

        //        post.TravelerName = travelerProfile.FullName;
        //        post.TravelerAvatar = travelerProfile.Profiles.ImageUser;
        //        post.LocalName = localProfile.FullName;
        //        post.LocalAvatar = localProfile.Profiles.ImageUser;
        //        await _pastTripPostDAO.UpdatePostAsync(post.Id, post);
        //    }

        //    return allPost;
        //}

        public async Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId)
        {
            var allPost = await _pastTripPostDAO.GetAllPostsAsync(userId);

            var travelerIds = allPost.Select(post => post.TravelerId).Distinct();
            var localIds = allPost.Where(post => post.LocalId.HasValue)
                                  .Select(post => post.LocalId.Value)
                                  .Distinct();

            var travelerProfiles = await _tourRepository.GetUsersInfoAsync(travelerIds);
            var localProfiles = await _tourRepository.GetUsersInfoAsync(localIds);

            var travelerProfileMap = travelerProfiles.ToDictionary(profile => profile.Id, profile => profile);
            var localProfileMap = localProfiles.ToDictionary(profile => profile.Id, profile => profile);

            foreach (var post in allPost)
            {
                if (travelerProfileMap.TryGetValue(post.TravelerId, out var travelerProfile))
                {
                    post.TravelerName = travelerProfile.FullName;
                    post.TravelerAvatar = travelerProfile.Profiles.ImageUser;
                }

                if (post.LocalId.HasValue && localProfileMap.TryGetValue(post.LocalId.Value, out var localProfile))
                {
                    post.LocalName = localProfile.FullName;
                    post.LocalAvatar = localProfile.Profiles.ImageUser;
                }
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

            //var participant = existingTour.Participants
            //                              .FirstOrDefault(item => item.ParticipantId == post.TravelerId);

            //if (participant != null && participant.PostId != post.Id)
            //{
            //    participant.PostId = post.Id;
            //    await _tourRepository.UpdateTour(existingTour.TourId, existingTour);
            //}
        }

        public async Task<double> GetUserAverageStar(int locaId)
        {
            var listPost = await _pastTripPostDAO.GetAllPostsAsync(locaId);

            var validPosts = listPost.Where(post => post.Star.HasValue && post.LocalId == locaId);
            var totalStars = validPosts.Sum(post => post.Star.Value);
            var postCount = validPosts.Count();

            return postCount > 0 ? totalStars / postCount : 0;
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
