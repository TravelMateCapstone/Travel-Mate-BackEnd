using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TravelMateAPI.Services.Storage
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly string _connectionString;

        public CloudStorageService(IConfiguration configuration)
        {
            //_connectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
            // Lấy chuỗi kết nối từ biến môi trường
            _connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        }

        public async Task<string> UploadFileAsync(string userId, Stream fileStream, string fileName, string contentType)
        {
            string containerName = "images";
            string filePath = $"{userId}/{fileName}"; // Đường dẫn lưu file

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            // Kiểm tra và tạo container nếu không tồn tại
            await containerClient.CreateIfNotExistsAsync();

            // Thiết lập quyền truy cập cho container (nếu cần)
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            // Tạo blob client để upload file
            BlobClient blobClient = containerClient.GetBlobClient(filePath);

            // Thiết lập header HTTP cho file
            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

            // Upload file lên blob
            await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

            return filePath;  // Trả về đường dẫn file sau khi upload
        }

        public async Task<string> UploadProfileImageAsync(string userId, Stream fileStream, string fileName, string contentType)
        {
            string containerName = "Profile";
            string filePath = $"{userId}/{fileName}"; // Đường dẫn lưu file

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            // Kiểm tra và tạo container nếu không tồn tại
            await containerClient.CreateIfNotExistsAsync();

            // Thiết lập quyền truy cập cho container (nếu cần)
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            // Tạo blob client để upload file
            BlobClient blobClient = containerClient.GetBlobClient(filePath);

            // Thiết lập header HTTP cho file
            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

            // Upload file lên blob
            await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

            return filePath;  // Trả về đường dẫn file sau khi upload
        }

        public async Task<string> GetFileUrlAsync(string userId, string fileName)
        {
            string containerName = "images";
            string filePath = $"{userId}/{fileName}"; // Đường dẫn lưu file

            // Khởi tạo BlobServiceClient và BlobClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobClient blobClient = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(filePath);

            // Kiểm tra xem blob có tồn tại không
            if (await blobClient.ExistsAsync())
            {
                // Trả về URL của blob
                return blobClient.Uri.ToString();
            }
            else
            {
                throw new FileNotFoundException("File không tồn tại");
            }
        }
        public async Task<List<string>> GetAllFilesAsync(string userId)
        {
            string containerName = "images";
            string prefix = $"{userId}/"; // Sử dụng prefix để chỉ lấy ảnh của user

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            List<string> fileUrls = new List<string>();

            // Lấy danh sách blobs có prefix tương ứng
            await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix))
            {
                fileUrls.Add(containerClient.GetBlobClient(blobItem.Name).Uri.ToString());
            }

            return fileUrls; // Trả về danh sách URL của các file
        }

    }
}
