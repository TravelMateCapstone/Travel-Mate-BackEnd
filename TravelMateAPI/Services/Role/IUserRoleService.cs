namespace TravelMateAPI.Services.Role
{
    public interface IUserRoleService
    {
        Task UpdateRoleToUserAsync(int userId);
        Task UpdateRoleToLocalAsync(int userId);
        Task UpdateRoleToTravelerAsync(int userId);
        //Task<List<string>> GetUserRolesAsync(int userId);
        Task<string> GetUserRoleAsync(int userId);
    }

}
