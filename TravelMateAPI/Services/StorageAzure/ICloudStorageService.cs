namespace TravelMateAPI.Services.StorageAzure
{
    public interface ICloudStorageService
    {
        Task<string> UploadFileAsync(string userId, Stream fileStream, string fileName, string contentType);
        Task<string> GetFileUrlAsync(string userId, string fileName);
        Task<List<string>> GetAllFilesAsync(string userId);
    }
}
