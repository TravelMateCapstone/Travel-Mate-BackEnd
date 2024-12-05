using BusinessObjects;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
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
            //var normalizedQuery = query.Trim().ToLower();
            // Chuẩn hóa query không dấu
            var normalizedQuery = RemoveDiacritics(query.Trim().ToLower());
            // Tính toán độ tương tự giữa query và từng LocationName
            var results = locations
                .Where(l => Fuzz.Ratio(normalizedQuery, RemoveDiacritics(l.LocationName.ToLower())) > 70) // Ngưỡng tương tự 70%
                .Select(l => new LocationDTO
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName
                })
                .ToList();

            return results;
        }
        // Hàm loại bỏ dấu
        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
