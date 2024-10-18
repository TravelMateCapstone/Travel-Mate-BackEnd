using BussinessObjects;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;
namespace TravelMateAPI.Services.FindLocal
{
    public class SearchLocationFuzzyService
    {
        private readonly ApplicationDBContext _context;

        public SearchLocationFuzzyService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<LocationDTO>> SearchLocationsAsync(string query)
        {
            var locations = await _context.Locations.ToListAsync();
            var normalizedQuery = query.Trim().ToLower();

            // Tính toán độ tương tự giữa query và từng LocationName
            var results = locations
                .Where(l => Fuzz.Ratio(normalizedQuery, l.LocationName.ToLower()) > 70) // Ngưỡng tương tự 70%
                .Select(l => new LocationDTO
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName
                })
                .ToList();

            return results;
        }
    }
}
