using BusinessObjects.Entities;
using DataAccess;

namespace Repositories
{
    public class PostCommentRepository : IPostCommentRepository
    {
        private readonly PostCommentDAO _postCommentDAO;

        public PostCommentRepository(PostCommentDAO postCommentDAO)
        {
            _postCommentDAO = postCommentDAO;
        }

        public async Task<IEnumerable<PostComment>> GetAllAsync(int postId)
        {
            return await _postCommentDAO.GetAll(postId);
        }

        public async Task<PostComment> GetByIdAsync(int commentId)
        {
            return await _postCommentDAO.GetGroupPostCommentById(commentId);
        }

        public async Task<PostComment> AddAsync(PostComment postComment)
        {
            return await _postCommentDAO.AddGroupPostCommentAsync(postComment);
        }

        public async Task DeleteAsync(int commentId)
        {
            await _postCommentDAO.DeleteGroupPostAsync(commentId);
        }

        public async Task UpdateAsync(PostComment postComment)
        {
            await _postCommentDAO.UpdateGroupPostDetailAsync(postComment);
        }

        public Task<bool> IsGroupMember(int groupId, int userId)
        {
            return _postCommentDAO.IsGroupMember(groupId, userId);
        }

        public Task<bool> IsCommentCreator(int commentId, int userId)
        {
            return _postCommentDAO.IsCommentCreator(commentId, userId);
        }
    }
}
