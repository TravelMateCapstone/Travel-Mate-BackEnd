using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface IUserProfileRepository
    {
        Task<List<UserProfile>> GetAllProfilesAsync();
        Task<UserProfile> GetProfileByIdAsync(int userId);
        Task<UserProfile> AddProfileAsync(UserProfile newProfile);
        Task UpdateProfileAsync(UserProfile updatedProfile);
        Task DeleteProfileAsync(int userId);
    }

}
