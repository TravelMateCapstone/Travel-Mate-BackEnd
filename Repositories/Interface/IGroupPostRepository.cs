using BusinessObjects.Entities;

namespace Repositories.Interface
{
    public interface IGroupPostRepository
    {
        Task<IEnumerable<GroupPost>> GetGroupPostsAsync(int groupId);
        Task<GroupPost> GetGroupPostByIdAsync(int postId);

        Task<bool> IsGroupPostCreator(int postId, int userId);
        Task<bool> IsPostExistInGroup(int groupId, int postId);
        Task AddAsync(GroupPost groupPost);
        Task UpdateAsync(GroupPost updatedGroupPost);
        Task DeleteAsync(int postId);
    }
}
