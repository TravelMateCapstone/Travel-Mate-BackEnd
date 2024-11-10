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
            return _dbContext.Groups.Include(g => g.GroupParticipants).OrderByDescending(g => g.CreateAt);
        }

        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
        }

        public async Task<IQueryable<Group>> GetUnjoinedGroupsAsync(int userId)
        {
            return _dbContext.Groups.Include(g => g.GroupParticipants)
                .Where(g => !g.GroupParticipants.Any(p => p.UserId == userId && p.JoinedStatus) && g.CreatedById != userId)
                .OrderByDescending(g => g.CreateAt);
        }

        public async Task<IQueryable<Group>> GetCreatedGroupsAsync(int userId)
        {
            return _dbContext.Groups
                .Where(g => g.CreatedById == userId)
                .OrderByDescending(g => g.CreateAt);
        }

        public async Task<Group> GetCreatedGroupByIdAsync(int userId, int groupId)
        {
            return await _dbContext.Groups
                .Where(g => g.CreatedById == userId)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
        }

        //Get Joined Group
        public async Task<IQueryable<Group>> GetJoinedGroupsAsync(int userId)
        {
            return _dbContext.Groups
        .Where(g => g.GroupParticipants.Any(gp => gp.UserId == userId && gp.JoinedStatus) && g.CreatedById != userId)
        .OrderByDescending(g => g.CreateAt);
        }

        public async Task<Group> GetJoinedGroupByIdAsync(int userId, int groupId)
        {
            return await _dbContext.Groups
                    .Include(g => g.GroupParticipants)
                    .FirstOrDefaultAsync(g => g.GroupId == groupId && g.GroupParticipants.Any(gp => gp.UserId == userId && gp.JoinedStatus));
        }

        public async Task<IQueryable<GroupParticipant>> ListJoinGroupRequests(int groupId)
        {
            return _dbContext.GroupParticipants
                .Include(g => g.User)
                .ThenInclude(g => g.Profiles)
                .Where(g => g.GroupId == groupId && !g.JoinedStatus)
                .OrderByDescending(g => g.RequestAt);
        }

        public async Task<bool> DoesRequestSend(int groupId, int userId)
        {
            return await _dbContext.GroupParticipants.AnyAsync(g => g.GroupId == groupId && g.UserId == userId && !g.JoinedStatus);
        }

        public async Task<IQueryable<GroupParticipant>> GetGroupMembers(int groupId)
        {
            return _dbContext.GroupParticipants
                .Include(g => g.User)
                .ThenInclude(g => g.Profiles)
                .Where(g => g.GroupId == groupId && g.JoinedStatus)
                .OrderByDescending(g => g.JoinAt);
        }

        public async Task<GroupParticipant> GetGroupMember(int userId, int groupId)
        {
            return await _dbContext.GroupParticipants
                .Include(g => g.Group)
                .FirstOrDefaultAsync(g => g.UserId == userId && g.GroupId == groupId);
        }
        //Ask to join a group
        public async Task JoinGroup(GroupParticipant newParticipant)
        {
            if (newParticipant == null)
            {
                throw new ArgumentNullException(nameof(newParticipant));
            }
            await _dbContext.GroupParticipants.AddAsync(newParticipant);
            await _dbContext.SaveChangesAsync();
        }

        //Leave a group
        public async Task LeaveGroup(GroupParticipant groupParticipant)
        {
            if (groupParticipant == null)
            {
                throw new ArgumentNullException(nameof(groupParticipant));
            }
            _dbContext.GroupParticipants.Remove(groupParticipant);
            await _dbContext.SaveChangesAsync();
        }


        //Group creator accept join 
        public async Task AcceptJoinGroup(GroupParticipant groupParticipant)
        {
            if (groupParticipant == null)
            {
                throw new ArgumentNullException(nameof(groupParticipant));
            }
            _dbContext.GroupParticipants.Update(groupParticipant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RejectJoinGroupRequest(GroupParticipant groupParticipant)
        {
            if (groupParticipant == null)
            {
                throw new ArgumentNullException(nameof(groupParticipant));
            }
            _dbContext.GroupParticipants.Remove(groupParticipant);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<GroupParticipant> GetJoinRequestParticipant(int userId, int groupId)
        {
            return await _dbContext.GroupParticipants
                .Include(g => g.Group)
                .FirstOrDefaultAsync(g => g.UserId == userId && g.GroupId == groupId && !g.JoinedStatus);
        }


        //create a group
        public async Task<Group> AddAsync(Group group)
        {
            group.NumberOfParticipants += 1;
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();
            return group;
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
