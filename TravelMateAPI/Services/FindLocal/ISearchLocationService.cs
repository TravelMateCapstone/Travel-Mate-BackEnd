using DataAccess;

namespace TravelMateAPI.Services.FindLocal
{
    public interface ISearchLocationService
    {
        Task<List<LocationDTO>> SearchLocationsAsync(string query);
    }
}
