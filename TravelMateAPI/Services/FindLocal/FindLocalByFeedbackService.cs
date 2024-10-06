using Microsoft.Data.SqlClient;
using Dapper;

namespace TravelMateAPI.Services.FindLocal
{
    public class FindLocalByFeedbackService :IFindLocalByFeedbackService
    {
        private readonly string _connectionString;

        public FindLocalByFeedbackService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        // Lấy danh sách người dùng local theo địa điểm và đánh giá trung bình từ cao đến thấp
        //public async Task<List<LocalFeedbackDTO>> GetLocalsByFeedbackAsync(int locationId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        // Câu truy vấn SQL lấy người dùng local và đánh giá trung bình của họ
        //        var sql1 = @"
        //        SELECT u.Id, u.FullName, u.Email, 
        //               AVG(f.Rate) AS AverageRate, 
        //               COUNT(f.FeedbackId) AS TotalFeedbacks
        //        FROM Users u
        //        INNER JOIN UserLocations ul ON u.Id = ul.UserId
        //        LEFT JOIN Feedback f ON u.Id = f.LocalUserId
        //        WHERE ul.LocationId = @LocationId
        //        GROUP BY u.Id, u.FullName, u.Email
        //        HAVING COUNT(f.FeedbackId) > 0  -- Chỉ lấy những người dùng có ít nhất một đánh giá
        //        ORDER BY AVG(f.Rate) DESC;  -- Sắp xếp theo đánh giá trung bình từ cao đến thấp";

        //        // Câu truy vấn SQL lấy toàn bộ người dùng local, bao gồm cả những người không có đánh giá
        //        var sql2 = @"
        //        SELECT u.Id, u.FullName, u.Email, 
        //               ISNULL(AVG(f.Rate), 0) AS AverageRate,  -- Nếu không có đánh giá, đặt giá trị trung bình là 0
        //               COUNT(f.FeedbackId) AS TotalFeedbacks
        //        FROM Users u
        //        INNER JOIN UserLocations ul ON u.Id = ul.UserId
        //        LEFT JOIN Feedback f ON u.Id = f.LocalId
        //        WHERE ul.LocationId = @LocationId
        //        GROUP BY u.Id, u.FullName, u.Email
        //        ORDER BY AVG(f.Rate) DESC;  -- Sắp xếp theo đánh giá trung bình từ cao đến thấp";

        //        // Thực thi truy vấn bằng Dapper
        //        var localUsers = await connection.QueryAsync<LocalFeedbackDTO>(sql2, new
        //        {
        //            LocationId = locationId
        //        });

        //        return localUsers.ToList();
        //    }
        //}

        // Lấy danh sách người dùng local theo địa điểm, đánh giá trung bình từ cao đến thấp, với phân trang
        public async Task<List<LocalFeedbackDTO>> GetLocalsByFeedbackAsync(int locationId, int pageNumber, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Tính toán số lượng bản ghi để bỏ qua
                var offset = (pageNumber - 1) * pageSize;

                // Câu truy vấn SQL với phân trang
                var sql = @"
                SELECT u.Id, u.FullName, u.Email, 
                       ISNULL(AVG(f.Rate), 0) AS AverageRate,  -- Nếu không có đánh giá, đặt giá trị trung bình là 0
                       COUNT(f.FeedbackId) AS TotalFeedbacks
                FROM Users u
                INNER JOIN UserLocations ul ON u.Id = ul.UserId
                LEFT JOIN Feedback f ON u.Id = f.LocalId
                WHERE ul.LocationId = @LocationId
                GROUP BY u.Id, u.FullName, u.Email
                ORDER BY AVG(f.Rate) DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";  // Phân trang: bỏ qua @Offset hàng và lấy @PageSize hàng

                // Thực thi truy vấn bằng Dapper
                var localUsers = await connection.QueryAsync<LocalFeedbackDTO>(sql, new
                {
                    LocationId = locationId,
                    Offset = offset,
                    PageSize = pageSize
                });

                return localUsers.ToList();
            }
        }

    }
}
