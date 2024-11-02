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
        }

        //Get UnJoin Group
        public async Task<IQueryable<Group>> GetGroupsAsync()
        {
            return _dbContext.Groups.Include(g => g.GroupParticipants);
        }

        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
        }

        public async Task<IQueryable<Group>> GetCreatedGroupsAsync(int userId)
        {
            return _dbContext.Groups
                .Where(g => g.CreatedById == userId);
        }

        public async Task<Group> GetCreatedGroupByIdAsync(int userId, int groupId)
        {
            return await _dbContext.Groups
                //.Include(g => g.GroupParticipants)
                .Where(g => g.CreatedById == userId)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
        }

        //Get Joined Group
        public async Task<IQueryable<Group>> GetJoinedGroupsAsync(int userId)
        {
            return _dbContext.Groups
        //.Include(g => g.GroupParticipants)
        .Where(g => g.GroupParticipants.Any(gp => gp.UserId == userId && gp.JoinedStatus));
        }

        public async Task<Group> GetJoinedGroupByIdAsync(int userId, int groupId)
        {
            return await _dbContext.Groups
                    .Include(g => g.GroupParticipants)
                    .FirstOrDefaultAsync(g => g.GroupId == groupId && g.GroupParticipants.Any(gp => gp.UserId == userId && gp.JoinedStatus));
        }

        public async Task<IQueryable<GroupParticipant>> ListJoinGroupRequests(int groupId)
        {
            return _dbContext.GroupParticipants.Where(g => g.GroupId == groupId && g.JoinedStatus == false);
        }

        //Ask to join a group
        public async Task JoinGroup(int userId, int groupId)
        {
            var newParticipant = new GroupParticipant
            {
                UserId = userId,
                GroupId = groupId,
                JoinedStatus = false
            };

            await _dbContext.GroupParticipants.AddAsync(newParticipant);
            await _dbContext.SaveChangesAsync();
        }

        //Leave a group
        public async Task LeaveGroup(int userId, int groupId)
        {
            var findGroupParticipant = await _dbContext.GroupParticipants.FindAsync(userId, groupId);
            if (findGroupParticipant != null)
            {
                _dbContext.GroupParticipants.Remove(findGroupParticipant);
                var group = await _dbContext.Groups.FindAsync(groupId);
                group.NumberOfParticipants -= 1;
                await _dbContext.SaveChangesAsync();
            }
        }


        //Group creator accept join 
        public async Task AcceptJoinGroup(int userId, int groupId)
        {
            var updateParticipantStatus = new GroupParticipant()
            {
                UserId = userId,
                GroupId = groupId,
                JoinedStatus = true
            };

            _dbContext.GroupParticipants.Update(updateParticipantStatus);
            var group = await _dbContext.Groups.FindAsync(groupId);

            group.NumberOfParticipants += 1;
            await _dbContext.SaveChangesAsync();
        }

        //create a group
        public async Task<Group> AddAsync(int userId, Group group)
        {

            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();
            var GetGroup = await GetLatestGroupAsync();
            var firstParticipant = new GroupParticipant()
            {
                UserId = userId,
                GroupId = GetGroup.GroupId,
                JoinedStatus = true
            };
            group.NumberOfParticipants += 1;
            await _dbContext.GroupParticipants.AddAsync(firstParticipant);
            await _dbContext.SaveChangesAsync();
            return group;
        }
        public async Task<Group> GetLatestGroupAsync()
        {
            return await _dbContext.Groups
                .OrderByDescending(g => g.CreateAt)  // Use CreatedDate if it exists
                .FirstOrDefaultAsync();
        }


        //creator delete a group
        public async Task DeleteAsync(int groupId)
        {
            var findGroup = await _dbContext.Groups.FindAsync(groupId);
            if (findGroup != null)
            {
                _dbContext.Groups.Remove(findGroup);
                await _dbContext.SaveChangesAsync();
            }
        }

        //group admin can update group information
        public async Task UpdateAsync(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            _dbContext.Groups.Update(group);
            await _dbContext.SaveChangesAsync();
        }
    }
}
