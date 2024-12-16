using Azure.Storage.Blobs;
using BusinessObjects.Configuration;
using MongoDB.Driver.Core.Configuration;
using TravelMateAPI.Services.Email;

namespace TravelMateAPI.MLModels
{
    public class BlobService
    {
        private readonly AppSettings _appSettings;
        private readonly string _connectionString ;
        private readonly string _containerName = "mlmodels";

        public BlobService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _connectionString = _appSettings.AzureStorage.AzureStorageConnectionString;
        }

        public async Task DownloadBlobAsync(string blobName, string downloadFilePath)
        {
            
            var blobClient = new BlobClient(_connectionString, _containerName, blobName);
            await blobClient.DownloadToAsync(downloadFilePath);
        }

        public async Task UploadBlobAsync(string blobName, string localFilePath)
        {
            var blobClient = new BlobClient(_connectionString, _containerName, blobName);

            // Tải tệp lên và ghi đè nếu đã tồn tại
            await blobClient.UploadAsync(localFilePath, overwrite: true);
        }
    }
}
