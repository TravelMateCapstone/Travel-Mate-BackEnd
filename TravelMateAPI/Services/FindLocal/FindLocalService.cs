using BussinessObjects.Entities;
using Microsoft.Data.SqlClient;
using Repositories.Interface;
using Dapper;

namespace TravelMateAPI.Services.FindLocal
{
    public class FindLocalService : IFindLocalService
    {
        private readonly string _connectionString;

        // Sử dụng IConfiguration để lấy chuỗi kết nối từ cấu hình
        public FindLocalService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        //public async Task<List<ApplicationUser>> GetMatchingUsersAsync(int locationId, List<int> activityIds, string roleName)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        // Tạo câu truy vấn SQL tương ứng
        //        var sql = @"
        //        SELECT u.Id, u.FullName, u.Email, u.RegistrationTime, COUNT(ua.ActivityId) AS MatchingActivitiesCount
        //        FROM Users u
        //        INNER JOIN UserRoles ur ON u.Id = ur.UserId
        //        INNER JOIN Roles r ON ur.RoleId = r.Id
        //        INNER JOIN UserLocations ul ON u.Id = ul.UserId
        //        INNER JOIN UserActivities ua ON u.Id = ua.UserId
        //        WHERE r.Name = @RoleName
        //          AND ul.LocationId = @LocationId
        //          AND ua.ActivityId IN @ActivityIds
        //        GROUP BY u.Id, u.FullName, u.Email, u.RegistrationTime
        //        ORDER BY COUNT(ua.ActivityId) DESC;";  // Sắp xếp theo số lượng hoạt động khớp

        //        // Sử dụng Dapper để thực hiện truy vấn
        //        var users = await connection.QueryAsync<ApplicationUser, int, ApplicationUser>(
        //            sql,
        //            (user, matchingActivitiesCount) =>
        //            {
        //                user.MatchingActivitiesCount = matchingActivitiesCount;
        //                return user;
        //            },
        //            new
        //            {
        //                RoleName = roleName,
        //                LocationId = locationId,
        //                ActivityIds = activityIds  // Dapper sẽ tự động xử lý danh sách cho lệnh IN
        //            },
        //            splitOn: "MatchingActivitiesCount"
        //        );

        //        return users.ToList();
        //    }
        //}


        //Có phân trang 
        public async Task<List<ApplicationUser>> GetMatchingUsersAsync(int locationId, List<int> activityIds, string roleName, int pageNumber, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Tính toán số lượng bản ghi để bỏ qua
                var offset = (pageNumber - 1) * pageSize;

                // Tạo câu truy vấn SQL với phân trang
                var sql = @"
            SELECT u.Id, u.FullName, u.Email, u.RegistrationTime, COUNT(ua.ActivityId) AS MatchingActivitiesCount
            FROM Users u
            INNER JOIN UserRoles ur ON u.Id = ur.UserId
            INNER JOIN Roles r ON ur.RoleId = r.Id
            INNER JOIN UserLocations ul ON u.Id = ul.UserId
            INNER JOIN UserActivities ua ON u.Id = ua.UserId
            WHERE r.Name = @RoleName
              AND ul.LocationId = @LocationId
              AND ua.ActivityId IN @ActivityIds
            GROUP BY u.Id, u.FullName, u.Email, u.RegistrationTime
            ORDER BY COUNT(ua.ActivityId) DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";  // Phân trang

                // Thực thi truy vấn bằng Dapper
                var users = await connection.QueryAsync<ApplicationUser, int, ApplicationUser>(
                    sql,
                    (user, matchingActivitiesCount) =>
                    {
                        user.MatchingActivitiesCount = matchingActivitiesCount;
                        return user;
                    },
                    new
                    {
                        RoleName = roleName,
                        LocationId = locationId,
                        ActivityIds = activityIds,
                        Offset = offset,         // Giá trị để bỏ qua
                        PageSize = pageSize      // Kích thước trang
                    },
                    splitOn: "MatchingActivitiesCount"
                );

                return users.ToList();
            }
        }
    }
}
