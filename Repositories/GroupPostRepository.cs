using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class GroupPostRepository : IGroupPostRepository
    {
        private readonly GroupPostDAO _groupPostDAO;

        public GroupPostRepository(GroupPostDAO groupPostDAO)
        {
            _groupPostDAO = groupPostDAO;
        }

        public async Task AddAsync(GroupPost groupPost)
        {
            await _groupPostDAO.AddGroupPostAsync(groupPost);
        }

        public async Task DeleteAsync(int postId)
        {
            await _groupPostDAO.DeleteGroupPostAsync(postId);
        }
        public async Task<GroupPost> GetGroupPostByIdAsync(int postId)
        {
            return await _groupPostDAO.GetGroupPostById(postId);
        }

        public async Task<IEnumerable<GroupPost>> GetGroupPostsAsync(int groupId)
        {
            return await _groupPostDAO.GetAll(groupId);
        }

        public async Task<bool> IsGroupPostCreator(int postId, int userId)
        {
            return await _groupPostDAO.IsGroupPostCreator(postId, userId);
        }

        public async Task<bool> IsPostExistInGroup(int groupId, int postId)
        {
            return await _groupPostDAO.IsPostExistInGroup(groupId, postId);
        }

        public async Task UpdateAsync(GroupPost updatedGroupPost)
        {
            await _groupPostDAO.UpdateGroupPostDetailAsync(updatedGroupPost);
        }

    }
}
