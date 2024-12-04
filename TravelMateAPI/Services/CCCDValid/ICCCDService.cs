using BusinessObjects.Entities;

namespace TravelMateAPI.Services.CCCDValid
{
    public interface ICCCDService
    {
        Task<CCCD?> GetByUserIdAsync(int userId);
        Task<bool> IsVerifiedAsync(int userId);
        Task<bool> IsPublicSignatureVerifiedAsync(int userId);
        Task<VerificationResult> VerifyAllAsync(int userId);
        Task<bool> VerifyDigitalSignatureAsync(int userId, string publicKey);
    }
}
