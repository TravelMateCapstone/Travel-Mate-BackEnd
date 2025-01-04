using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace TravelMateAPI.Services.Blockchain
{
    public class BlockchainService
    {
        private readonly ApplicationDBContext _dbContext;

        public BlockchainService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task GenerateKeysForUserAsync(int userId)
        {
            // Tạo cặp private-public key cho user
            var keys = CryptoGenerateKeys(); // Sử dụng thư viện để tạo key
            var blockchainKey = new BlockchainKey
            {
                UserId = userId,
                PublicKey = keys.PublicKey,
                PrivateKey = EncryptPrivateKey(keys.PrivateKey) // Bảo vệ private key
            };

            _dbContext.BlockchainKeys.Add(blockchainKey);
            await _dbContext.SaveChangesAsync();
        }

        public string SignData(string privateKey, string data)
        {
            return CryptoSignData(privateKey, data);
        }

        public bool VerifySignature(string publicKey, string data, string signature)
        {
            return CryptoVerifySignature(publicKey, data, signature);
        }

        private string EncryptPrivateKey(string privateKey)
        {
            // Mã hóa private key để lưu an toàn
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(privateKey));
        }

        private string DecryptPrivateKey(string encryptedKey)
        {
            // Giải mã private key khi sử dụng
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encryptedKey));
        }

        public async Task<string> GetUserPublicKeyAsync(int userId)
        {
            var key = await _dbContext.BlockchainKeys.FirstOrDefaultAsync(k => k.UserId == userId);
            if (key == null) throw new Exception("Public key not found for user");

            return key.PublicKey;
        }

        public async Task<string> GetUserPrivateKeyAsync(int userId)
        {
            var key = await _dbContext.BlockchainKeys.FirstOrDefaultAsync(k => k.UserId == userId);
            if (key == null) throw new Exception("Private key not found for user");

            return DecryptPrivateKey(key.PrivateKey);
        }


        //Crypto
        /// <summary>
        /// Generates a new RSA key pair.
        /// </summary>
        public static (string PublicKey, string PrivateKey) CryptoGenerateKeys()
        {
            using (var rsa = RSA.Create())
            {
                return (
                    PublicKey: Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                    PrivateKey: Convert.ToBase64String(rsa.ExportRSAPrivateKey())
                );
            }
        }

        /// <summary>
        /// Signs data using the given private key.
        /// </summary>
        public static string CryptoSignData(string privateKey, string data)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                var dataBytes = Encoding.UTF8.GetBytes(data);
                var signatureBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(signatureBytes);
            }
        }

        /// <summary>
        /// Verifies a signature using the given public key.
        /// </summary>
        public static bool CryptoVerifySignature(string publicKey, string data, string signature)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                var dataBytes = Encoding.UTF8.GetBytes(data);
                var signatureBytes = Convert.FromBase64String(signature);
                return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }

}
