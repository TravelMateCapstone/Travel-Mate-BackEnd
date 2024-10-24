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

        public async Task AddAsync(Group group)
        {
            await _groupDAO.AddGroupAsync(group);
        }

        public async Task DeleteAsync(int Id)
        {
            await _groupDAO.DeleteGroupAsync(Id);
        }


        public async Task<Group> GetByIdAsync(int Id)
        {
            return await _groupDAO.GetGroupById(Id);
        }

        public async Task UpdateAsync(Group group)
        {
            await _groupDAO.UpdateGroupDetailAsync(group);
        }

        public async Task<IEnumerable<Group>> GetAll()
        {
            return await _groupDAO.GetAll();
        }

    }
}
