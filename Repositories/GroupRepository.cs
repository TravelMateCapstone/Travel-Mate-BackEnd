using DataAccess;
using Repositories.Interface;

namespace Repositories
{
    public class GroupRepository : IGroupRepository
    {

        private readonly GroupDAO _groupDAO;

        public GroupRepository(GroupDAO groupDAO)
        {
            _groupDAO = groupDAO;
        }

        public async Task AcceptJoinGroup(int userId, int groupId)
        {
            await _groupDAO.AcceptJoinGroup(userId, groupId);
        }

        public async Task AddAsync(Group group)
        {
            await _groupDAO.AddAsync(group);
        }

        public async Task<int> CountGroupParticipants(int groupId)
        {
            return await _groupDAO.GroupParticipantCount(groupId);
        }

        public async Task DeleteAsync(int Id)
        {
            await _groupDAO.DeleteAsync(Id);
        }


        public async Task<Group> GetCreatedGroupByIdAsync(int userId, int groupId)
        {
            return await _groupDAO.GetCreatedGroupByIdAsync(userId, groupId);
        }

        public async Task<IQueryable<Group>> GetCreatedGroupsAsync(int createdById)
        {
            return await _groupDAO.GetCreatedGroupsAsync(createdById);
        }

        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await _groupDAO.GetGroupByIdAsync(groupId);
        }

        public async Task<IQueryable<Group>> GetGroupsAsync()
        {
            return await _groupDAO.GetGroupsAsync();
        }

        public async Task<Group> GetJoinedGroupByIdAsync(int userId, int groupId)
        {
            return await _groupDAO.GetJoinedGroupByIdAsync(userId, groupId);
        }

        public async Task<IQueryable<Group>> GetJoinedGroupsAsync(int userId)
        {
            return await _groupDAO.GetJoinedGroupsAsync(userId);
        }

        public async Task JoinGroup(int userId, int groupId)
        {
            await _groupDAO.JoinGroup(userId, groupId);
        }

        public async Task LeaveGroup(int userId, int groupId)
        {
            await _groupDAO.LeaveGroup(userId, groupId);
        }

        public async Task UpdateAsync(Group group)
        {
            await _groupDAO.UpdateAsync(group);
        }


    }
}
