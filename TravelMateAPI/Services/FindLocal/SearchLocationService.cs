using BussinessObjects;
using Microsoft.EntityFrameworkCore;

namespace TravelMateAPI.Services.FindLocal
{
    public class SearchLocationService : ISearchLocationService
    {
        private readonly ApplicationDBContext _context;

        public SearchLocationService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<LocationDTO>> SearchLocationsAsync(string query)
        {
            var normalizedQuery = query.Trim().ToLower();

            return await _context.Locations
                .Where(l => EF.Functions.Like(l.LocationName.ToLower(), $"%{normalizedQuery}%"))
                .Select(l => new LocationDTO
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName
                })
                .ToListAsync();
        }
    }
}
