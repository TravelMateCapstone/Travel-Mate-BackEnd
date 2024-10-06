namespace TravelMateAPI.Services.FindLocal
{
    public interface IFindLocalByFeedbackService
    {
        Task<List<LocalFeedbackDTO>> GetLocalsByFeedbackAsync(int locationId, int pageNumber, int pageSize);

    }
}
