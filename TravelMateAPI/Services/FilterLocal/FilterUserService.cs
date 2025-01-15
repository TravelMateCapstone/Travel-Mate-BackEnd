using BusinessObjects;
using BusinessObjects.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace TravelMateAPI.Services.FilterLocal
{
    public class FilterUserService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPastTripPostRepository _postRepository;
        private readonly IContractService _contractService;
        private readonly TourDAO _tourDAO;

        public FilterUserService(ApplicationDBContext context, UserManager<ApplicationUser> userManager, IPastTripPostRepository postRepository, IContractService contractService, TourDAO tourDAO)
        {
            _context = context;
            _userManager = userManager;
            _postRepository = postRepository;
            _contractService = contractService;
            _tourDAO = tourDAO;
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

        public async Task<List<UserWithDetailsDTO>> GetAllUsersWithDetailsAsync(int userId) //List<int> targetActivityIds, int minAge, int maxAge, string sex
        {

            //var users = await _context.Users
            //    .Include(u => u.Profiles)
            //    .Include(u => u.CCCDs).ToListAsync(); // Include CCCD for DoB and Sex
            // Lấy danh sách Users trừ người dùng hiện tại
            var users = await _context.Users
                .Where(u => u.Id != userId && u.Id != 126) // Loại bỏ người dùng hiện tại
                .Include(u => u.Profiles)
                .Include(u => u.CCCDs)
                .ToListAsync();

            var result = new List<UserWithDetailsDTO>();

            foreach (var user in users)
            {
                // Lấy danh sách roles
                var roles = await _userManager.GetRolesAsync(user);

                // Lấy CCCD
                var cccd = user.CCCDs;
                var star = await _postRepository.GetUserAverageStar(user.Id);
                var countConnect = await _contractService.GetContractCountAsLocalAsync(user.Id);
                //// Lấy danh sách ActivityId của người dùng
                //var userActivities = await GetUserActivityIdsAsync(user.Id);
                //var userLocations = await GetUserLocationIdsAsync(user.Id);

                // Lấy danh sách ActivityId của người dùng
                var userActivities = await GetUserActivityNameAsync(user.Id);
                var userLocations = await GetUserLocationNameAsync(user.Id);





                //var tours = await _tourDAO.GetTourBriefByLocalId(user.Id);
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
                    Star = star,
                    CountConnect = countConnect,
                    Profile = user.Profiles == null ? null : new ProfileDTO
                    {
                        ProfileId = user.Profiles.ProfileId,
                        Address = user.Profiles.Address,
                        ImageUser = user.Profiles.ImageUser
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

                    ActivityIds = userActivities.ToList(),
                    //SimilarityScore = similarityScore // Thêm điểm tương tự
                    // Tours = tours // Gắn danh sách tour vào DTO
                });
            }

            return result;
            // Sắp xếp giảm dần theo SimilarityScore
            //return result.OrderByDescending(u => u.SimilarityScore).ToList();
        }


        public async Task<List<UserWithDetailsDTO>> GetAllUsersWithDetailsByRoleAsync(string role)
        {
            // Lấy danh sách người dùng thuộc vai trò cụ thể
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);

            var result = new List<UserWithDetailsDTO>();

            foreach (var user in usersInRole)
            {
                // Include thông tin Profile, CCCD, và các liên kết khác
                var detailedUser = await _context.Users
                    .Include(u => u.Profiles)
                    .Include(u => u.CCCDs)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                if (detailedUser == null) continue;

                // Lấy CCCD
                var cccd = detailedUser.CCCDs;

                // Lấy số sao và số lần kết nối
                var star = await _postRepository.GetUserAverageStar(user.Id);
                var countConnect = await _contractService.GetContractCountAsLocalAsync(user.Id);

                // Lấy tên hoạt động và địa điểm của người dùng
                var userActivities = await GetUserActivityNameAsync(user.Id);
                var userLocations = await GetUserLocationNameAsync(user.Id);

                result.Add(new UserWithDetailsDTO
                {
                    UserId = detailedUser.Id,
                    FullName = detailedUser.FullName,
                    Email = detailedUser.Email,
                    LocationIds = userLocations.ToList(),
                    Roles = new List<string> { role }, // Đã biết người dùng thuộc vai trò nào
                    Star = star,
                    CountConnect = countConnect,
                    Profile = detailedUser.Profiles == null ? null : new ProfileDTO
                    {
                        ProfileId = detailedUser.Profiles.ProfileId,
                        Address = detailedUser.Profiles.Address,
                        ImageUser = detailedUser.Profiles.ImageUser
                    },
                    CCCD = cccd == null ? null : new CCCDDTO
                    {
                        Dob = cccd.dob,
                        Sex = cccd.sex,
                        Age = CalculateAge(cccd.dob)  // Tính tuổi từ dob và gán vào DTO
                    },
                    ActivityIds = userActivities.ToList(),
                });
            }

            return result;
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

        public async Task<List<string>> GetUserActivityNameAsync(int userId)
        {
            var activityName = await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.Activity.ActivityName)
                .ToListAsync();

            return activityName;
        }
        public async Task<List<string>> GetUserLocationNameAsync(int userId)
        {
            var locationName = await _context.UserLocations
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.Location.LocationName)
                .ToListAsync();

            return locationName;
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


        public async Task<List<UserWithDetailNoToursDTO>> GetAllUsersWithDetailsByIdsAsync(List<int> userIds) //List<int> targetActivityIds, int minAge, int maxAge, string sex
        {
            var users = await _context.Users
            .Include(u => u.Profiles)
            .Include(u => u.CCCDs)
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

            // Lấy danh sách roles cho từng user
            var rolesDictionary = new Dictionary<int, IList<string>>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                rolesDictionary[user.Id] = userRoles;
            }

            // Kết hợp thông tin từ các nguồn khác
            var result = new List<UserWithDetailNoToursDTO>();

            foreach (var user in users)
            {
                // Lấy Star và CountConnect
                var star = await _postRepository.GetUserAverageStar(user.Id);
                var countConnect = await _contractService.GetContractCountAsLocalAsync(user.Id);

                //// Lấy ActivityIds và LocationIds
                //var userActivities = await GetUserActivityIdsAsync(user.Id);
                //var userLocations = await GetUserLocationIdsAsync(user.Id);

                // Lấy danh sách ActivityId của người dùng
                var userActivities = await GetUserActivityNameAsync(user.Id);
                var userLocations = await GetUserLocationNameAsync(user.Id);


                // Tạo DTO
                var userDto = new UserWithDetailNoToursDTO
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    LocationIds = userLocations.ToList(),
                    Roles = rolesDictionary[user.Id].ToList(),
                    Star = star,
                    CountConnect = countConnect,
                    Profile = user.Profiles == null ? null : new ProfileDTO2
                    {
                        ProfileId = user.Profiles.ProfileId,
                        Address = user.Profiles.Address,
                        ImageUser = user.Profiles.ImageUser
                    },
                    CCCD = user.CCCDs == null ? null : new CCCDDTO2
                    {
                        Dob = user.CCCDs.dob,
                        Sex = user.CCCDs.sex,
                        Age = CalculateAge(user.CCCDs.dob) // Tính tuổi
                    },
                    ActivityIds = userActivities.ToList()
                };

                result.Add(userDto);
            }

            return result;
        }
    }
}
