using BusinessObjects;
using BusinessObjects.Entities;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;

//namespace TravelMateAPI.Services.Contract
//{
    public class ContractService : IContractService
{
        //private readonly List<ContractDTO> _contractsInMemory;
        private readonly ApplicationDBContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private const string ContractsCacheKey = "ContractsInMemory";

        public ContractService(ApplicationDBContext dbContext, IMemoryCache memoryCache)
        {
            //_contractsInMemory = new List<ContractDTO>();
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<ContractDTO> CreateContract(int travelerId, int localId, string tourId, string details, string status, string travelerSignature, string localSignature)
        {
            var newContract = new ContractDTO
            {
                TravelerId = travelerId,
                LocalId = localId,
                TourId = tourId,
                Details = details,
                Status = status,
                TravelerSignature = HashWithSecretKey(travelerSignature),
                LocalSignature = HashWithSecretKey(localSignature),
                CreatedAt = GetTimeZone.GetVNTimeZoneNow()
            };
            // Lấy danh sách hợp đồng từ bộ nhớ cache
            var contractsInMemory = _memoryCache.GetOrCreate(ContractsCacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return new List<ContractDTO>();
            });

            // Thêm hợp đồng mới vào bộ nhớ
            contractsInMemory.Add(newContract);

            // Cập nhật lại cache
            _memoryCache.Set(ContractsCacheKey, contractsInMemory);

            return newContract;

            //_contractsInMemory.Add(newContract);
            //return newContract;
        }

        public ContractDTO FindContractInMemory(int travelerId, int localId, string tourId)
        {
            //return _contractsInMemory.FirstOrDefault(c =>
            //    c.TravelerId == travelerId && c.LocalId == localId && c.TourId == tourId);
            if (_memoryCache.TryGetValue(ContractsCacheKey, out List<ContractDTO> contractsInMemory))
            {
                return contractsInMemory.FirstOrDefault(c =>
                    c.TravelerId == travelerId && c.LocalId == localId && c.TourId == tourId);
            }

            return null;
        }

        public void UpdateStatusToCompleted(int travelerId, int localId, string tourId)
        {
            var contract = FindContractInMemory(travelerId, localId, tourId);
            if (contract == null)
            {
                throw new Exception("Hợp đồng không tồn tại trong bộ nhớ.");
            }

            if (contract.Status == "Created")
            {
                contract.Status = "Completed";
            }
            else
            {
                throw new Exception("Chỉ hợp đồng ở trạng thái 'Created' mới có thể chuyển thành 'Completed'.");
            }
        }

        public void UpdateStatusToCancelled(int travelerId, int localId, string tourId)
        {
            var contract = FindContractInMemory(travelerId, localId, tourId);
            if (contract == null)
            {
                throw new Exception("Hợp đồng không tồn tại trong bộ nhớ.");
            }

            if (contract.Status == "Created")
            {
                contract.Status = "Cancelled";
            }
            else
            {
                throw new Exception("Chỉ hợp đồng ở trạng thái 'Created' mới có thể chuyển thành 'Cancelled'.");
            }
        }

        public async Task SaveContractToDatabase(int travelerId, int localId, string tourId)
        {
            // Tìm hợp đồng trong bộ nhớ
            var dto = FindContractInMemory(travelerId, localId, tourId);
            if (dto == null)
            {
                throw new Exception("Hợp đồng không tồn tại trong bộ nhớ.");
            }

            // Kiểm tra trạng thái
            if (dto.Status != "Completed")
            {
                throw new Exception("Chỉ hợp đồng ở trạng thái 'Completed' mới được lưu vào database.");
            }

            // Lấy PreviousHash từ hợp đồng mới nhất trong database
            var lastContract = await _dbContext.BlockContracts.OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync();
            string previousHash = lastContract?.Hash ?? string.Empty;

            // Tạo đối tượng Contract
            var newContract = new BlockContract
            {
                Id = Guid.NewGuid().ToString(),
                TravelerId = dto.TravelerId,
                LocalId = dto.LocalId,
                TourId = dto.TourId,
                Details = dto.Details,
                Status = dto.Status,
                TravelerSignature = dto.TravelerSignature,
                LocalSignature = dto.LocalSignature,
                CreatedAt = dto.CreatedAt,
                PreviousHash = previousHash
            };

            // Tính toán Hash
            newContract.Hash = CalculateHash(newContract);

            // Lưu vào database
            _dbContext.BlockContracts.Add(newContract);
            await _dbContext.SaveChangesAsync();
        }

        private string CalculateHash(BlockContract contract)
        {
            var rawData = $"{contract.Id}{contract.TravelerId}{contract.LocalId}{contract.TourId}" +
                          $"{contract.Details}{contract.Status}{contract.TravelerSignature}" +
                          $"{contract.LocalSignature}{contract.CreatedAt}{contract.PreviousHash}";

            //using (var sha256 = SHA256.Create())
            //{
            //    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            //    return Convert.ToBase64String(bytes);
            //}
            return HashWithSecretKey(rawData);
        }

        private string HashWithSecretKey(string data)
        {
            var secretKey = "DAcaumongmoidieutotdep8386"; // Lấy khóa bí mật từ file env
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Secret key is missing from configuration.");
            }

            using var hmac = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var hashedBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashedBytes);
        }

        public async Task<bool> VerifyContractIntegrityAsync(int travelerId, int localId, string tourId)
        {
            // Tìm hợp đồng dựa trên các thông tin đầu vào
            var contract = await _dbContext.BlockContracts
                .FirstOrDefaultAsync(c => c.TravelerId == travelerId &&
                                          c.LocalId == localId &&
                                          c.TourId == tourId);

            if (contract == null)
            {
                throw new InvalidOperationException("Hợp đồng không tồn tại.");
            }

            // Tính lại hash từ dữ liệu hợp đồng
            var recalculatedHash = CalculateHash(contract);

            // So sánh hash đã tính với hash đã lưu
            return recalculatedHash == contract.Hash;
        }

        public async Task<int> GetContractCountAsLocalAsync(int userId)
        {
            // Đếm số lượng hợp đồng có LocalId bằng userId
            var contractCount = await _dbContext.BlockContracts
                .CountAsync(c => c.LocalId == userId);

            return contractCount;
        }


    }
//}
