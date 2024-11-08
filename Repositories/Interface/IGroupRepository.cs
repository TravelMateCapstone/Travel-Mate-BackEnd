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
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int Id);


        // User actions related to groups
        Task JoinGroup(int userId, int groupId);
        Task LeaveGroup(GroupParticipant groupParticipant);

        // Accept a user's request to join the group
        Task AcceptJoinGroup(GroupParticipant groupParticipant);
        Task RejectJoinGroupRequest(GroupParticipant groupParticipant);

        Task<IEnumerable<GroupParticipant>> ListJoinGroupRequests(int groupId);
        Task<IEnumerable<GroupParticipant>> GetGroupMembers(int groupId);

        Task<GroupParticipant> GetGroupMember(int userId, int groupId);

        Task<GroupParticipant> GetJoinRequestParticipant(int userId, int groupId);

        Task<bool> DoesRequestSend(int groupId, int userId);
    }
}
