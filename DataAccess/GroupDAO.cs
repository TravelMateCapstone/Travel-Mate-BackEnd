using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class GroupDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public GroupDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            //inject dbcontext
        }

        public async Task<IEnumerable<Group>> GetAll()
        {
            return _dbContext.Groups.Include(g => g.GroupParticipants).ToList();
        }

        public async Task<Group> GetGroupById(int id)
        {
            return await _dbContext.Groups.Include(g => g.GroupParticipants).FirstOrDefaultAsync(g => g.GroupId == id);
        }

        public async Task<Group> AddGroupAsync(Group group)
        {
            _dbContext.Groups.Add(group);
            await _dbContext.SaveChangesAsync();
            return group;

        }
        public async Task DeleteGroupAsync(int groupId)
        {

            var findGroup = await _dbContext.Groups.FindAsync(groupId);
            if (findGroup != null)
            {
                _dbContext.Groups.Remove(findGroup);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateGroupDetailAsync(Group group)
        {
            _dbContext.Groups.Update(group);
            await _dbContext.SaveChangesAsync();
        }
    }
}
