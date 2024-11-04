namespace Repositories.Interface
{
    public interface IGroupRepository
    {
        Task<IQueryable<Group>> GetCreatedGroupsAsync(int createdById);
        Task<Group> GetCreatedGroupByIdAsync(int userId, int groupId);

        Task<IQueryable<Group>> GetJoinedGroupsAsync(int userId);
        Task<Group> GetJoinedGroupByIdAsync(int userId, int groupId);

        Task<IQueryable<Group>> GetGroupsAsync();
        Task<IQueryable<Group>> GetUnjoinedGroupsAsync(int userId);
        Task<Group> GetGroupByIdAsync(int groupId);

        //Group operations
        Task AddAsync(int userId, Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int Id);


        // User actions related to groups
        Task JoinGroup(int userId, int groupId);
        Task LeaveGroup(int userId, int groupId);

        // Accept a user's request to join the group
        Task AcceptJoinGroup(int userId, int groupId);

        Task<IEnumerable<GroupParticipant>> ListJoinGroupRequests(int groupId);
        Task<IEnumerable<GroupParticipant>> GetGroupMembers(int groupId);

        Task<bool> DoesRequestSend(int groupId, int userId);
    }
}
