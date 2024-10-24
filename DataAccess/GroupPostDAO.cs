using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class GroupPostDAO
    {

        private readonly ApplicationDBContext _dbContext;

        public GroupPostDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GroupPost>> GetAll()
        {
            return _dbContext.GroupPosts.Include(g => g.Comments).Include(g => g.PostPhotos).ToList();
        }

        //public async Task<GroupPost> GetGroupPostById(int id)
        //{
        //    //return await _dbContext.GroupPosts.Include(g => g.Comments).Include(g => g.PostPhotos).FirstOrDefaultAsync(g => g.GroupPostId == id);
        //}

        public async Task<GroupPost> AddGroupPostAsync(GroupPost GroupPost)
        {
            _dbContext.GroupPosts.Add(GroupPost);
            await _dbContext.SaveChangesAsync();
            return GroupPost;

        }
        public async Task DeleteGroupPostAsync(int GroupPostId)
        {

            var findGroupPost = await _dbContext.GroupPosts.FindAsync(GroupPostId);
            if (findGroupPost != null)
            {
                _dbContext.GroupPosts.Remove(findGroupPost);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateGroupPostDetailAsync(GroupPost GroupPost)
        {
            _dbContext.GroupPosts.Update(GroupPost);
            await _dbContext.SaveChangesAsync();
        }
    }
}
