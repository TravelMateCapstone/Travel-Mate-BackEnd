using BusinessObjects.Entities;

namespace Repositories
{
    public interface IPostCommentRepository
    {
        Task<IEnumerable<PostComment>> GetAllAsync(int postId);
        Task<PostComment> GetByIdAsync(int commentId);
        Task<PostComment> AddAsync(PostComment postComment);
        Task DeleteAsync(int commentId);
        Task UpdateAsync(PostComment postComment);
        Task<bool> IsGroupMember(int groupId, int userId);
        Task<bool> IsCommentCreator(int commentId, int userId);

    }
}
