namespace Repositories.Interface
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAll();
        Task<Group> GetByIdAsync(int Id);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int Id);

    }
}
