using BusinessObjects.Entities;
using Repositories.Interface;
using System.Text;
using System.Security.Cryptography;
using TravelMateAPI.Services.CCCDValid;

//namespace TravelMateAPI.Services.CCCDValid
//{
    public class CCCDService : ICCCDService
    {
        private readonly ICCCDRepository _repository;

        public CCCDService(ICCCDRepository repository)
        {
            _repository = repository;
        }

        // Lấy CCCD theo UserId
        public async Task<CCCD?> GetByUserIdAsync(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        // Kiểm tra mặt trước đã được xác minh hay chưa
        public async Task<bool> IsVerifiedAsync(int userId)
        {
            var cccd = await GetByUserIdAsync(userId);
            if (cccd == null) return false;

            return !(string.IsNullOrEmpty(cccd.imageFront) ||
                     string.IsNullOrEmpty(cccd.id) ||
                     string.IsNullOrEmpty(cccd.name) ||
                     string.IsNullOrEmpty(cccd.dob) ||
                     string.IsNullOrEmpty(cccd.sex) ||
                     string.IsNullOrEmpty(cccd.nationality) ||
                     string.IsNullOrEmpty(cccd.home) ||
                     string.IsNullOrEmpty(cccd.address) ||
                     string.IsNullOrEmpty(cccd.doe)) || string.IsNullOrEmpty(cccd.imageBack) ||
                 string.IsNullOrEmpty(cccd.features) ||
                 string.IsNullOrEmpty(cccd.issue_date) ||
                 cccd.mrz == null || !cccd.mrz.Any() ||
                 string.IsNullOrEmpty(cccd.issue_loc);
        }

        // Kiểm tra chữ ký số đã xác minh
        public async Task<bool> IsPublicSignatureVerifiedAsync(int userId)
        {
            var cccd = await GetByUserIdAsync(userId);
            if (cccd == null) return false;

            return !string.IsNullOrEmpty(cccd.PublicSignature);
        }


        public async Task<VerificationResult> VerifyAllAsync(int userId)
        {
            var cccd = await GetByUserIdAsync(userId);
            if (cccd == null)
            {
                return new VerificationResult
                {
                    IsVerified = false,
                    Message = "Không tìm thấy CCCD.",

                    IsPublicSignatureVerified = false,
                    PublicSignatureMessage = "Không tìm thấy Chữ ký số."
                };
            }

            return new VerificationResult
            {
                IsVerified = await IsVerifiedAsync(userId),
                Message = await IsVerifiedAsync(userId)
                    ? "CCCD đã được xác minh."
                    : "CCCD chưa được xác minh.",

                IsPublicSignatureVerified = await IsPublicSignatureVerifiedAsync(userId),
                PublicSignatureMessage = await IsPublicSignatureVerifiedAsync(userId)
                    ? "Chữ ký số đã được xác minh."
                    : "Chữ ký số chưa được xác minh."
            };
        }

        public async Task<bool> VerifyDigitalSignatureAsync(int userId, string publicKey)
        {
            // Lấy CCCD của userId từ database
            var cccd = await _repository.GetByUserIdAsync(userId);
            if (cccd == null || string.IsNullOrEmpty(cccd.PublicSignature))
            {
                throw new Exception("Chữ ký số không tồn tại hoặc người dùng chưa tạo chữ ký số.");
            }

            var secretKey = "DAcaumongmoidieutotdep8386"; // Lấy khóa bí mật từ file env
            // Tạo mã băm từ publicKey được truyền vào
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(publicKey));
            var computedHashString = Convert.ToBase64String(computedHash);

            // So sánh mã băm được tính với mã băm đã lưu trong bảng CCCD
            return computedHashString == cccd.PublicSignature;
        }

        // Kiểm tra chữ ký số đã xác minh
        public async Task<string> GetPrivateSignatureAsync(int userId)
        {
            var cccd = await GetByUserIdAsync(userId);
            if (cccd == null) return null;

            return cccd.PublicSignature ;
        }
}
//}
