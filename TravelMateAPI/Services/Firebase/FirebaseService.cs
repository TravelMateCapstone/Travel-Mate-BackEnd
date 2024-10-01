using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TravelMateAPI.Services.Firebase
{
    public class FirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseService(IConfiguration config)
        {
            // Khởi tạo Firebase Admin SDK với tệp JSON credentials (hoặc từ cấu hình .env)
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("path-to-your-firebase-adminsdk-json-file.json"),
            });

            // Lấy thông tin từ cấu hình .env (được inject qua DI)
            _bucketName = config["FIREBASE_STORAGE_BUCKET"];

            _storageClient = StorageClient.Create();
        }

        // Phương thức để upload hình ảnh lên Firebase Storage
        public async Task<string> UploadFileAsync(IFormFile file, string userId)
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
