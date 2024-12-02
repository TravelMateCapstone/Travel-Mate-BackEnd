using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using BusinessObjects.Utils.Response;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Encoders;

namespace TravelMateAPI.Services.FilterLocal
{
    public class FilterUserService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FilterUserService(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //        public async Task<List<UserWithDetailsDTO>> GetAllUsersWithDetailsAsync()
        //        {
        //            var users = await _context.Users
        //                .Include(u => u.Profiles)
        //                .Include(u => u.CCCDs) // Liên kết với CCCD
        //                .Include(u => u.UserActivities) // Liên kết với UserActivity
        //                    .ThenInclude(ua => ua.Activity).ToListAsync();
        //                //.Include(u => u.PastTripPosts) // Liên kết với PastTripPost
        //                //.ToListAsync();

        //            var result = new List<UserWithDetailsDTO>();

        //            foreach (var user in users)
        //            {
        //                // Lấy vai trò của người dùng
        //                var roles = await _userManager.GetRolesAsync(user);

        //                // Lấy thông tin từ CCCD
        //                var cccd = user.CCCDs;

        //                // Lấy danh sách ActivityId từ UserActivity
        //                var userActivities = user.UserActivities?.Select(ua => ua.ActivityId).ToList();

        //                // Lấy danh sách Star từ PastTripPost (ReviewById = user.Id)
        //                var pastTripStars = user.PastTripPosts
        //                    .Where(ptp => ptp.ReviewById == user.Id)
        //                    .Select(ptp => ptp.Star)
        //                    .ToList();

        //                // Tính trung bình Star
        //                double? averageStar = pastTripStars.Any() ? pastTripStars.Average() : null;

        //                //        // Tính tuổi từ dob
        //                //        int? age = null;
        //                //        if (DateTime.TryParse(cccd?.dob, out var dob))
        //                //        {
        //                //            var now = DateTime.Now;
        //                //            age = now.Year - dob.Year;
        //                //            if (dob > now.AddYears(-age.Value)) age--; // Điều chỉnh nếu chưa đến sinh nhật
        //                //        }

        //            //    int age = !string.IsNullOrEmpty(cccd?.dob)
        //            //? (int)((DateTime.Now - DateTime.Parse(cccd?.dob)).TotalDays / 365.25)
        //            //: 0;

        //        result.Add(new UserWithDetailsDTO
        //                {
        //                    UserId = user.Id,
        //                    FullName = user.FullName,
        //                    Email = user.Email,
        //                    Roles = roles.ToList(),
        //                    Profile = user.Profiles == null ? null : new ProfileDTO
        //                    {
        //                        ProfileId = user.Profiles.ProfileId,
        //                        Address = user.Profiles.Address
        //    },
        //                    Dob = cccd?.dob,
        //                    Sex = cccd?.sex,
        //                    Age = age,
        //                    UserActivities = userActivities
        //                    //PastTripStars = pastTripStars,
        //                    //AverageStar = averageStar
        //});
        //            }

        //            return result;
        //        }

        //public async Task<List<UserWithDetailsDTO>> GetAllUsersWithDetailsAsync()
        //{
        //    // Lấy danh sách user cùng thông tin liên quan
        //    var users = await _context.Users
        //        .Include(u => u.CCCDs) // Bao gồm CCCD để lấy dob và sex
        //        .Include(u => u.UserActivities).ThenInclude(ua => ua.Activity) // Bao gồm UserActivities để lấy danh sách ActivityId
        //        .ToListAsync();
        //    //.Include(u => u.PastTripPosts) // Bao gồm PastTripPosts để lấy danh sách Star

        //    // Ánh xạ sang DTO
        //    var userDtos = users.Select(u => new UserWithDetailsDTO
        //    {
        //        UserId = u.Id,
        //        FullName = u.FullName,
        //        Email = u.Email,
        //        Roles = _userManager.GetRolesAsync(u).Result.ToList(),
        //        Profile = u.Profiles != null ? new ProfileDTO
        //        {
        //            ProfileId = u.Profiles.ProfileId,
        //            Address = u.Profiles.Address
        //        } : null,
        //        Dob = u.CCCDs?.dob,
        //        Sex = u.CCCDs?.sex,
        //        Age = !string.IsNullOrEmpty(u.CCCDs?.dob)
        //            ? (int?)((DateTime.Now - DateTime.Parse(u.CCCDs.dob)).TotalDays / 365.25)
        //            : null,
        //        UserActivities = u.UserActivities?.Select(ua => ua.ActivityId).ToList()
        //        //PastTripStars = u.PastTripPosts?.Select(pt => pt.Star).ToList(),
        //        //AverageStar = u.PastTripPosts != null && u.PastTripPosts.Any()
        //        //    ? u.PastTripPosts.Average(pt => pt.Star)
        //        //    : null
        //    }).ToList();

        //    return userDtos;
        //}

        public async Task<List<UserWithDetailsDTO>> GetAllUsersWithDetailsAsync() //List<int> targetActivityIds, int minAge, int maxAge, string sex
        {
            var users = await _context.Users
                .Include(u => u.Profiles)
                .Include(u => u.CCCDs).ToListAsync(); // Include CCCD for DoB and Sex
               

            var result = new List<UserWithDetailsDTO>();

            foreach (var user in users)
            {
                // Lấy danh sách roles
                var roles = await _userManager.GetRolesAsync(user);

                // Lấy CCCD
                var cccd = user.CCCDs;

                // Lấy danh sách ActivityId của người dùng
                var userActivities = await GetUserActivityIdsAsync(user.Id);
                var userLocations = await GetUserLocationIdsAsync(user.Id);
                // Tính tuổi và kiểm tra giới tính
                //var age = CalculateAge(cccd?.dob);
                //if (age < minAge || age > maxAge || cccd?.sex != sex)
                //    continue; // Bỏ qua người dùng nếu không thỏa mãn điều kiện tuổi và giới tính

                // Tính mức độ tương tự với targetActivityIds
                //var similarityScore = CalculateSimilarityScore(userActivities, targetActivityIds);


                // Thêm vào DTO
                result.Add(new UserWithDetailsDTO
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    LocationIds = userLocations.ToList(),
                    Roles = roles.ToList(),
                    Profile = user.Profiles == null ? null : new ProfileDTO
                    {
                        ProfileId = user.Profiles.ProfileId,
                        Address = user.Profiles.Address
                    },
                    CCCD = cccd == null ? null : new CCCDDTO
                    {
                        Dob = cccd.dob,
                        Sex = cccd.sex,
                        Age = CalculateAge(cccd.dob)  // Tính tuổi từ dob và gán vào DTO
                        //Age=age 
                    },
                    //UserActivities = new UserActivitiesDTO
                    //{
                    //    ActivityIds = userActivities
                    //}
                    
                     ActivityIds = userActivities.ToList()
                     //SimilarityScore = similarityScore // Thêm điểm tương tự
                });
            }

            return result;
            // Sắp xếp giảm dần theo SimilarityScore
            //return result.OrderByDescending(u => u.SimilarityScore).ToList();
        }

        public async Task<List<int>> GetUserActivityIdsAsync(int userId)
        {
            var activityIds = await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.ActivityId)
                .ToListAsync();

            return activityIds;
        }
        public async Task<List<int>> GetUserLocationIdsAsync(int userId)
        {
            var locationIds = await _context.UserLocations
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.LocationId)
                .ToListAsync();

            return locationIds;
        }

        private int CalculateAge(string dob)
        {
            if (string.IsNullOrEmpty(dob))
                return 0;

            DateTime birthDate;
            if (DateTime.TryParse(dob, out birthDate))
            {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;
                if (birthDate.Date > today.AddYears(-age)) age--; // Kiểm tra nếu sinh nhật chưa qua trong năm nay
                return age;
            }
            return 0; // Trả về 0 nếu không thể phân tích ngày sinh
        }

        private int CalculateSimilarityScore(List<int> userActivityIds, List<int> targetActivityIds)
        {
            if (userActivityIds == null || targetActivityIds == null)
                return 0;

            // Tính số lượng phần tử chung giữa hai danh sách
            return userActivityIds.Intersect(targetActivityIds).Count();
        }
    }
}
