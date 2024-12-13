using BusinessObjects.Entities;
namespace TravelMateAPI.Services.ReportUser
{
    public interface IUserReportService
    {
        Task<List<UserReport>> GetAllReportsAsync();
        Task<UserReport?> GetReportByIdAsync(int reportId);
        Task<UserReport> CreateReportAsync(UserReport report);
        Task<UserReport?> UpdateReportAsync(int reportId, UserReport updatedReport);
        Task<bool> DeleteReportAsync(int reportId);
        Task<UserReport?> UpdateReportStatusAsync(int reportId, string newStatus);

    }

}
