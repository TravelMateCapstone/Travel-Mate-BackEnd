using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using TravelMateAPI.Services.FilterTour;

namespace TravelMateAPI.Services.RecommenTourService
{
    public class RecommenTourService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly FilterTourService _filterTourService;

        public RecommenTourService(ApplicationDBContext context,FilterTourService filterTourService)
        {
            _dbContext = context;
            _filterTourService = filterTourService;
        }

        /// <summary>
        /// Lấy danh sách TravelerId đi kèm danh sách TourId và ActivityId từ UserActivity.
        /// </summary>
        /// <returns>Danh sách các TravelerId cùng với danh sách TourId và ActivityId.</returns>
        //public async Task<List<TravelerActivityTourDTO>> GetTravelerToursAndActivitiesAsync()
        //{
        //    // Truy vấn danh sách BlockContracts để lấy TourIds
        //    var contractData = await _dbContext.BlockContracts
        //        .GroupBy(b => b.TravelerId)
        //        .Select(g => new
        //        {
        //            TravelerId = g.Key,
        //            TourIds = g.Select(c => c.TourId).Distinct().ToList()
        //        })
        //        .ToListAsync();

        //    // Truy vấn danh sách UserActivity để lấy ActivityIds
        //    var userActivities = await _dbContext.UserActivities
        //        .GroupBy(ua => ua.UserId)
        //        .Select(g => new
        //        {
        //            UserId = g.Key,
        //            ActivityIds = g.Select(ua => ua.ActivityId).Distinct().ToList()
        //        })
        //        .ToListAsync();

        //    // Ghép dữ liệu từ cả hai nguồn
        //    var result = contractData
        //        .Select(cd => new TravelerActivityTourDTO
        //        {
        //            TravelerId = cd.TravelerId,
        //            TourIds = cd.TourIds,
        //            ActivityIds = userActivities
        //                .FirstOrDefault(ua => ua.UserId == cd.TravelerId)?.ActivityIds ?? new List<int>()
        //        })
        //        .ToList();

        //    return result;
        //}

        public async Task<List<TravelerActivityTourDTO>> GetTravelerToursAndActivitiesAsync()
        {
            // Truy vấn danh sách BlockContracts để lấy TourIds và số lần mua
            var contractData = await _dbContext.BlockContracts
                .GroupBy(b => new { b.TravelerId, b.TourId })
                .Select(g => new
                {
                    g.Key.TravelerId,
                    g.Key.TourId,
                    PurchaseCount = g.Count()
                })
                .GroupBy(x => x.TravelerId)
                .Select(g => new
                {
                    TravelerId = g.Key,
                    TourData = g.Select(t => new TourPurchaseDTO
                    {
                        TourId = t.TourId,
                        PurchaseCount = t.PurchaseCount
                    }).ToList()
                })
                .ToListAsync();

            // Truy vấn danh sách UserActivity để lấy ActivityIds
            var userActivities = await _dbContext.UserActivities
                .GroupBy(ua => ua.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    ActivityIds = g.Select(ua => ua.ActivityId).Distinct().ToList()
                })
                .ToListAsync();

            // Ghép dữ liệu từ cả hai nguồn
            var result = contractData
                .Select(cd => new TravelerActivityTourDTO
                {
                    TravelerId = cd.TravelerId,
                    Tours = cd.TourData,
                    ActivityIds = userActivities
                        .FirstOrDefault(ua => ua.UserId == cd.TravelerId)?.ActivityIds ?? new List<int>()
                })
                .ToList();

            return result;
        }


        /// <summary>
        /// Đề xuất danh sách Tour cho TravelerId dựa trên Collaborative Filtering.
        /// </summary>
        /// <param name="travelerId">ID của traveler cần được gợi ý</param>
        /// <returns>Danh sách TourId được đề xuất</returns>
        public async Task<List<string>> GetRecommendedToursAsync(int travelerId, int same)
        {
            // 1. Lấy dữ liệu BlockContracts
            var allContracts = await _dbContext.BlockContracts
                .GroupBy(b => new { b.TravelerId, b.TourId })
                .Select(g => new
                {
                    g.Key.TravelerId,
                    g.Key.TourId,
                    PurchaseCount = g.Count()
                })
                .ToListAsync();

            if (allContracts == null || !allContracts.Any())
            {
                return new List<string>(); // Trả về danh sách rỗng nếu không có dữ liệu
            }
            // Bước 2: Kiểm tra nếu travelerId không tồn tại trong danh sách
            // 2. Chuyển đổi allContracts thành Dictionary
            var travelerTourDictionary = allContracts
                .GroupBy(c => c.TravelerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(c => new { c.TourId, c.PurchaseCount }).ToList()
                );
            if (!travelerTourDictionary.ContainsKey(travelerId))
            {
                // Xử lý khi travelerId chưa book tour nào
                return new List<string>(); // Trả về danh sách trống
            }

            // 2. Xây dựng ma trận TravelerId x TourId
            var travelerTours = allContracts
                .GroupBy(c => c.TravelerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(c => c.TourId, c => c.PurchaseCount)
                );

            // 3. Tìm các traveler tương tự
            var similarityScores = new Dictionary<int, double>();

            foreach (var otherTravelerId in travelerTours.Keys)
            {
                if (otherTravelerId == travelerId) continue;

                // Tính độ tương tự bằng Cosine Similarity
                double similarity = CalculateCosineSimilarity(
                    travelerTours[travelerId],
                    travelerTours[otherTravelerId]
                );

                similarityScores[otherTravelerId] = similarity;
            }

            // 4. Lấy danh sách các tour đã mua bởi các traveler tương tự
            var recommendedTours = new Dictionary<string, double>();

            foreach (var (otherTravelerId, similarity) in similarityScores
                .OrderByDescending(s => s.Value)
                .Take(same)) // Chỉ lấy top 2 traveler tương tự nhất
            {
                foreach (var (tourId, count) in travelerTours[otherTravelerId])
                {
                    // Nếu traveler hiện tại chưa mua tour này
                    if (!travelerTours[travelerId].ContainsKey(tourId))
                    {
                        if (!recommendedTours.ContainsKey(tourId))
                            recommendedTours[tourId] = 0;

                        // Tổng hợp điểm gợi ý dựa trên độ tương tự và số lần mua
                        recommendedTours[tourId] += similarity * count;
                    }
                }
            }

            // 5. Trả về danh sách các tour được đề xuất, sắp xếp theo điểm gợi ý
            return recommendedTours
                .OrderByDescending(r => r.Value)
                .Select(r => r.Key).Take(4)
                .ToList();
        }

        /// <summary>
        /// Tính độ tương tự giữa hai traveler dựa trên Cosine Similarity.
        /// </summary>
        private double CalculateCosineSimilarity(Dictionary<string, int> userA, Dictionary<string, int> userB)
        {
            var commonTours = userA.Keys.Intersect(userB.Keys).ToList();

            if (!commonTours.Any()) return 0;

            double dotProduct = commonTours.Sum(t => userA[t] * userB[t]);
            double magnitudeA = Math.Sqrt(userA.Values.Sum(v => v * v));
            double magnitudeB = Math.Sqrt(userB.Values.Sum(v => v * v));

            return magnitudeA > 0 && magnitudeB > 0 ? dotProduct / (magnitudeA * magnitudeB) : 0;
        }




        public async Task<List<TourWithUserDetailsDTO>> GetDetailedToursAsync(List<string> tourIds)
        {
            var detailedTours = new List<TourWithUserDetailsDTO>();

            foreach (var tourId in tourIds)
            {
                // Gọi phương thức lấy thông tin chi tiết từng tour
                var tourDetails = await _filterTourService.GetAllTourBriefWithUserDetailsByTourIdAsync(tourId);
                if (tourDetails != null)
                {
                    detailedTours.AddRange(tourDetails);
                }
            }

            return detailedTours;
        }


        public async Task<List<TourWithUserDetailsDTO>> GetRecommendedOrTopToursAsync(int travelerId, int same, int random)
        {
            // Lấy danh sách TourId được đề xuất
            var recommendedTourIds = await GetRecommendedToursAsync(travelerId, same);

            if (recommendedTourIds == null || !recommendedTourIds.Any())
            {
                // Nếu không có gợi ý, lấy top tour
                return await _filterTourService.GetAllTourBriefWithUserDetailsTopAsync(random);
            }

            // Lấy chi tiết từ danh sách TourId được đề xuất
            return await GetDetailedToursAsync(recommendedTourIds);
        }
    }
}
