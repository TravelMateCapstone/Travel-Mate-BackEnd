using BussinessObjects.Configuration;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace TravelMateAPI.Services.Firebase
{
    public class FirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseService(FirebaseConfig firebaseConfig)
        {
            if (string.IsNullOrEmpty(firebaseConfig.FirebaseAdminSdkJsonPath))
            {
                throw new InvalidOperationException("Firebase Admin SDK JSON path is not set.");
            }

            // Khởi tạo Firebase Admin SDK với tệp JSON credentials từ biến cấu hình
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(firebaseConfig.FirebaseAdminSdkJsonPath),
            });

            // Lấy thông tin bucket từ cấu hình
            _bucketName = firebaseConfig.StorageBucket;
            _storageClient = StorageClient.Create();
        }

        // Phương thức để upload hình ảnh lên Firebase Storage
        public async Task<string> UploadFileAsync(IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be empty");

            var fileName = $"{userId}/{Guid.NewGuid()}_{file.FileName}";
            using var stream = file.OpenReadStream();

            var imageObject = await _storageClient.UploadObjectAsync(_bucketName, fileName, null, stream);

            // Trả về URL của hình ảnh
            var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{imageObject.Name}";
            return publicUrl;
        }
    }
}
