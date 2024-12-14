using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace TravelMateAPI.Services.ReportUser
{
    public class UserReportService : IUserReportService
    {
        private readonly ApplicationDBContext _context;

        public UserReportService(ApplicationDBContext context)
        {
            _context = context;
        }

        // Lấy tất cả các báo cáo, bao gồm cả thông tin người dùng liên quan
        public async Task<List<UserReport>> GetAllReportsAsync()
        {
            return await _context.UserReports.ToListAsync();
        }

        // Lấy báo cáo theo ReportId
        public async Task<UserReport?> GetReportByIdAsync(int reportId)
        {
            return await _context.UserReports.FirstOrDefaultAsync(ur => ur.UserReportId == reportId);
        }

        // Tạo một báo cáo mới
        public async Task<UserReport> CreateReportAsync(UserReport report)
        {
            // Thêm báo cáo vào cơ sở dữ liệu
            _context.UserReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        // Cập nhật báo cáo theo ReportId
        public async Task<UserReport?> UpdateReportAsync(int reportId, UserReport updatedReport)
        {
            var existingReport = await _context.UserReports.FindAsync(reportId);
            if (existingReport == null)
            {
                return null; // Nếu không tìm thấy báo cáo, trả về null
            }

            // Cập nhật thông tin báo cáo (giữ nguyên các trường còn lại)
            existingReport.Detail = updatedReport.Detail ?? existingReport.Detail;
            existingReport.ImageReport = updatedReport.ImageReport ?? existingReport.ImageReport;
            existingReport.ReportType = updatedReport.ReportType ?? existingReport.ReportType;
            existingReport.Status = updatedReport.Status ?? existingReport.Status;

            await _context.SaveChangesAsync();
            return existingReport;
        }

        // Xóa báo cáo theo ReportId
        public async Task<bool> DeleteReportAsync(int reportId)
        {
            var report = await _context.UserReports.FindAsync(reportId);
            if (report == null)
            {
                return false; // Nếu không tìm thấy báo cáo, trả về false
            }

            _context.UserReports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        // Cập nhật trạng thái báo cáo
        public async Task<UserReport?> UpdateReportStatusAsync(int reportId, string newStatus)
        {
            var report = await _context.UserReports.FindAsync(reportId);
            if (report == null)
            {
                return null; // Nếu không tìm thấy báo cáo, trả về null
            }

            report.Status = newStatus; // Cập nhật chỉ trạng thái
            await _context.SaveChangesAsync();
            return report;
        }
    }


}
