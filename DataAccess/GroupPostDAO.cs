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

        public async Task<IEnumerable<GroupPost>> GetAll(int groupId)
        {
            return await _dbContext.GroupPosts
                .Where(p => p.GroupId == groupId)
                .Include(g => g.PostBy)
                .ThenInclude(g => g.Profiles)
                .Include(g => g.Comments)
                .Include(g => g.PostPhotos)
                .ToListAsync();
        }

        public async Task<GroupPost> GetGroupPostById(int id)
        {
            return await _dbContext.GroupPosts
                .Include(g => g.PostBy)
                .ThenInclude(g => g.Profiles)
                .Include(g => g.Comments)
                .Include(g => g.PostPhotos)
                .FirstOrDefaultAsync(g => g.PostId == id);
        }
        public async Task<bool> IsGroupPostCreator(int postId, int userId)
        {
            return await _dbContext.GroupPosts.AnyAsync(g => g.PostById == userId && g.PostId == postId);
        }

        public async Task<bool> IsPostExistInGroup(int groupId, int postId)
        {
            return await _dbContext.GroupPosts.AnyAsync(g => g.GroupId == groupId && g.PostId == postId);
        }
        //check if user belong to group
        public async Task<bool> IsMemberOrAdmin(int userId, int groupId)
        {
            return await _dbContext.Groups
                .AnyAsync(g => g.GroupId == groupId &&
                               (g.CreatedById == userId ||
                                g.GroupParticipants.Any(p => p.UserId == userId && p.GroupId == groupId && p.JoinedStatus)));
        }


        public async Task<GroupPost> AddGroupPostAsync(GroupPost groupPost)
        {
            await _dbContext.GroupPosts.AddAsync(groupPost);
            await _dbContext.SaveChangesAsync();
            return groupPost;
        }

        public async Task DeleteGroupPostAsync(int groupPostId)
        {
            var findGroupPost = await _dbContext.GroupPosts.FindAsync(groupPostId);
            if (findGroupPost != null)
            {
                _dbContext.GroupPosts.Remove(findGroupPost);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateGroupPostDetailAsync(GroupPost updatedGroupPost)
        {
            _dbContext.GroupPosts.Update(updatedGroupPost);
            await _dbContext.SaveChangesAsync();
        }
    }
}
