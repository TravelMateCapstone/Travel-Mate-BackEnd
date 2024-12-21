using BusinessObjects;
using BusinessObjects.Entities;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using TravelMateAPI.Services.Notification;
using TravelMateAPI.Services.Email;
using Google.Api;
using TravelMateAPI.Services.CCCDValid;
using TravelMateAPI.Services.Role;
using DataAccess;
using TravelMateAPI.Services.Contract;

//namespace TravelMateAPI.Services.Contract
//{
public class ContractService : IContractService
{
        //private readonly List<ContractDTO> _contractsInMemory;
        private readonly ApplicationDBContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private const string ContractsCacheKey = "ContractsInMemory";
        private readonly INotificationService _notificationService;
        private readonly IMailServiceSystem _mailService;
        private readonly ICCCDService _ccCDService;
        private readonly IUserRoleService _userRoleService;
        private readonly TourDAO _tourDAO;

        public ContractService(ApplicationDBContext dbContext, IMemoryCache memoryCache, INotificationService notificationService, IMailServiceSystem mailService, ICCCDService cCCDService,IUserRoleService userRoleService, TourDAO tourDAO)
        {
            //_contractsInMemory = new List<ContractDTO>();
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _notificationService = notificationService;
            _mailService = mailService;
            _ccCDService = cCCDService;
            _userRoleService = userRoleService;
            _tourDAO = tourDAO;
        }

        public async Task<ContractDTO> CreateContract(int travelerId, int localId, string tourId,string Location, string details, string status, string travelerSignature, string localSignature)
        {
            // Kiểm tra xem travelerId có phải là Local không
            var travelerRole = await _userRoleService.GetUserRoleAsync(travelerId);
            if (travelerRole == "Local")
            {
                throw new InvalidOperationException("Bạn không được đi du lịch trong thời gian làm người địa phương.");
            }


            // Kiểm tra xem hợp đồng đã tồn tại trong bảng BlockContracts chưa
            var existingContract = await _dbContext.BlockContracts
                .FirstOrDefaultAsync(c => c.TravelerId == travelerId && c.LocalId == localId && c.TourId == tourId);

            if (existingContract != null)
            {
                throw new InvalidOperationException("Hợp đồng với TravelerId, LocalId và TourId này đã tồn tại.");
            }

            var newContract = new ContractDTO
            {
                TravelerId = travelerId,
                LocalId = localId,
                TourId = tourId,
                Location = Location,
                Details = details,
                Status = status,
                TravelerSignature = HashWithSecretKey(travelerSignature),
                LocalSignature = HashWithSecretKey(localSignature),
                CreatedAt = GetTimeZone.GetVNTimeZoneNow()
            };
            // Lấy danh sách hợp đồng từ bộ nhớ cache
            var contractsInMemory = _memoryCache.GetOrCreate(ContractsCacheKey, entry =>
            {
                //entry.SlidingExpiration = TimeSpan.FromHours(1);
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                return new List<ContractDTO>();
            });

            // Thêm hợp đồng mới vào bộ nhớ
            contractsInMemory.Add(newContract);

            // Cập nhật lại cache
            _memoryCache.Set(ContractsCacheKey, contractsInMemory);
            var traveler = await _dbContext.Users.FindAsync(travelerId);
            var tourName = await _tourDAO.GetTourNameById(tourId);
            await _notificationService.CreateNotificationFullAsync(localId, $"Hợp đồng của chuyến đi ID:{tourName} của bạn đang được tạo bởi {traveler.FullName}.", travelerId, 5);
            return newContract;

            //_contractsInMemory.Add(newContract);
            //return newContract;
        }

        public async Task<ContractDTO> CreateContractPassLocal(int travelerId, int localId, string tourId, string Location, string details, string status, string travelerSignature)
        {
            // Kiểm tra xem travelerId có phải là Local không
            var travelerRole = await _userRoleService.GetUserRoleAsync(travelerId);
            if (travelerRole == "Local")
            {
                throw new InvalidOperationException("Bạn không được đi du lịch trong thời gian làm người địa phương.");
            }

            // Kiểm tra xem hợp đồng đã tồn tại trong bảng BlockContracts chưa
            var existingContract = await _dbContext.BlockContracts
                .FirstOrDefaultAsync(c => c.TravelerId == travelerId && c.LocalId == localId && c.TourId == tourId);

            if (existingContract != null)
            {
                throw new InvalidOperationException("Hợp đồng với TravelerId, LocalId và TourId này đã tồn tại.");
            }


            var newContract = new ContractDTO
            {
                TravelerId = travelerId,
                LocalId = localId,
                TourId = tourId,
                Location = Location,
                Details = details,
                Status = status,
                TravelerSignature = HashWithSecretKey(travelerSignature),
                LocalSignature = await _ccCDService.GetPrivateSignatureAsync(localId),
                CreatedAt = GetTimeZone.GetVNTimeZoneNow()
            };
            // Lấy danh sách hợp đồng từ bộ nhớ cache
            var contractsInMemory = _memoryCache.GetOrCreate(ContractsCacheKey, entry =>
            {
                //entry.SlidingExpiration = TimeSpan.FromHours(1);
                entry.SlidingExpiration = TimeSpan.FromMinutes(30);
                return new List<ContractDTO>();
            });

            // Thêm hợp đồng mới vào bộ nhớ
            contractsInMemory.Add(newContract);

            // Cập nhật lại cache
            _memoryCache.Set(ContractsCacheKey, contractsInMemory);
            var traveler = await _dbContext.Users.FindAsync(travelerId);
            var tourName = await _tourDAO.GetTourNameById(tourId);
            await _notificationService.CreateNotificationFullAsync(localId, $"Hợp đồng của chuyến đi ID:{tourName} của bạn đang được tạo bởi {traveler.FullName}.", travelerId, 5);
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

        public async Task UpdateStatusToCompleted(int travelerId, int localId, string tourId)
        {
            var contract = FindContractInMemory(travelerId, localId, tourId);
            if (contract == null)
            {
                throw new Exception("Hợp đồng không tồn tại trong bộ nhớ.");
            }
            
        if (contract.Status == "Created")
            {
                contract.Status = "Completed";
                await SaveContractToDatabase(travelerId, localId, tourId);
                Console.WriteLine("Hợp đồng đã được tạo thành công.");
                var traveler = await _dbContext.Users.FindAsync(travelerId);
                var local = await _dbContext.Users.FindAsync(localId);
                var tourName = await _tourDAO.GetTourNameById(tourId);
                await _notificationService.CreateNotificationFullAsync(localId, $"Hợp đồng của chuyến đi ID:{tourName} đã ký kết thành công cùng với Khách: {traveler.FullName}.", travelerId, 5);
                await _notificationService.CreateNotificationFullAsync(travelerId, $"Hợp đồng của chuyến đi ID:{tourName} đã ký kết thành công cùng với Người địa phương: {local.FullName}.", localId, 5);

                MailContent content1 = new MailContent
                {
                    To = local.Email,
                    Subject = "Thông báo Hợp Đồng - Travel Mate",
                    Body = $"Hợp đồng của chuyến đi ID:{tourId} đã ký kết thành công cùng với Khách:{traveler.FullName}."
                };
                MailContent content2 = new MailContent
                {
                    To = traveler.Email,
                    Subject = "Thông báo Hợp Đồng - Travel Mate",
                    Body = $"Hợp đồng của chuyến đi ID:{tourId} đã ký kết thành công cùng với Người địa phương: {local.FullName}."
                };
                await _mailService.SendMail(content1);
                await _mailService.SendMail(content2);

            //await _mailService.SendMail(content);
        }
            else
            {
                throw new Exception("Chỉ hợp đồng ở trạng thái 'Created' mới có thể chuyển thành 'Completed'.");
            }
        }

        public async Task UpdateStatusToCancelled(int travelerId, int localId, string tourId)
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
                Location = dto.Location,
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
            //update role
            await _userRoleService.UpdateRoleToLocalAsync(newContract.LocalId);
            await _userRoleService.UpdateRoleToTravelerAsync(newContract.TravelerId);
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

        public async Task<int> GetContractLocationCountAsync(string location)
        {
            // Đếm số lượng hợp đồng có LocalId bằng userId
            var contractCount = await _dbContext.BlockContracts
                .CountAsync(c => c.Location == location);

            return contractCount;
        }

    public async Task<List<TravelerContractDTO>> GetContractsByTravelerAsync(int travelerId)
    {
        // Lấy danh sách hợp đồng từ cơ sở dữ liệu dựa vào travelerId
        var dbContracts = await _dbContext.BlockContracts
            .Where(c => c.TravelerId == travelerId)
            .Select(c => new TravelerContractDTO
            {
                LocalId = c.LocalId,
                TourId = c.TourId,
                Location = c.Location,
                Details = c.Details,
                CreatedAt = c.CreatedAt,
                Status = c.Status,
                LocalProfile = _dbContext.Profiles
                    .Where(p => p.UserId == c.LocalId)
                    .Select(p => new ProfileDTO
                    {
                        Phone = p.Phone,
                        ImageUser = p.ImageUser,
                        Description = p.Description,
                        HostingAvailability = p.HostingAvailability
                    })
                    .FirstOrDefault(),
                Account = _dbContext.Users.Where(u => u.Id ==c.LocalId).Select(u => new AccountDTO
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email
                }).FirstOrDefault()

            })
            .ToListAsync();

        // Lấy danh sách hợp đồng từ MemoryCache
        var memoryContracts = _memoryCache.GetOrCreate(ContractsCacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return new List<ContractDTO>();
        })
        .Where(c => c.TravelerId == travelerId && c.Status == "Created")
        .Select(c => new TravelerContractDTO
        {
            LocalId = c.LocalId,
            TourId = c.TourId,
            Location = c.Location,
            Details = c.Details,
            CreatedAt = c.CreatedAt,
            Status = c.Status,
            LocalProfile = _dbContext.Profiles
                .Where(p => p.UserId == c.LocalId)
                .Select(p => new ProfileDTO
                {
                   
                    Phone = p.Phone,
                    ImageUser = p.ImageUser,
                    Description = p.Description,
                    HostingAvailability = p.HostingAvailability
                })
                .FirstOrDefault(),
            Account = _dbContext.Users.Where(u => u.Id == c.LocalId).Select(u => new AccountDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            }).FirstOrDefault()
        })
        .ToList();

        // Kết hợp danh sách từ cơ sở dữ liệu và bộ nhớ
        var allContracts = dbContracts.Concat(memoryContracts).ToList();

        return allContracts;
    }

    public async Task<List<LocalContractDTO>> GetContractsByLocalAsync(int localId)
    {
        // Lấy danh sách hợp đồng từ cơ sở dữ liệu dựa vào travelerId
        var dbContracts = await _dbContext.BlockContracts
            .Where(c => c.LocalId == localId)
            .Select(c => new LocalContractDTO
            {
                TravelerId = c.TravelerId,
                TourId = c.TourId,
                Location = c.Location,
                Details = c.Details,
                CreatedAt = c.CreatedAt,
                Status = c.Status,
                LocalProfile = _dbContext.Profiles
                    .Where(p => p.UserId == c.TravelerId)
                    .Select(p => new ProfileDTO
                    {
                        Phone = p.Phone,
                        ImageUser = p.ImageUser,
                        Description = p.Description,
                        HostingAvailability = p.HostingAvailability
                    })
                    .FirstOrDefault(),
                Account = _dbContext.Users.Where(u => u.Id == c.TravelerId).Select(u => new AccountDTO
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email
                }).FirstOrDefault()

            })
            .ToListAsync();

        // Lấy danh sách hợp đồng từ MemoryCache
        var memoryContracts = _memoryCache.GetOrCreate(ContractsCacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return new List<ContractDTO>();
        })
        .Where(c => c.LocalId == localId && c.Status == "Created")
        .Select(c => new LocalContractDTO
        {
            TravelerId = c.TravelerId,
            TourId = c.TourId,
            Location = c.Location,
            Details = c.Details,
            CreatedAt = c.CreatedAt,
            Status = c.Status,
            LocalProfile = _dbContext.Profiles
                .Where(p => p.UserId == c.TravelerId)
                .Select(p => new ProfileDTO
                {

                    Phone = p.Phone,
                    ImageUser = p.ImageUser,
                    Description = p.Description,
                    HostingAvailability = p.HostingAvailability
                })
                .FirstOrDefault(),
            Account = _dbContext.Users.Where(u => u.Id == c.TravelerId).Select(u => new AccountDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            }).FirstOrDefault()
        })
        .ToList();

        // Kết hợp danh sách từ cơ sở dữ liệu và bộ nhớ
        var allContracts = dbContracts.Concat(memoryContracts).ToList();

        return allContracts;
    }

    public async Task<string> CheckContractStatusAsync(int travelerId, string tourId)
    {
        // Kiểm tra trong bộ nhớ cache
        var contractsInMemory = _memoryCache.GetOrCreate(ContractsCacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            return new List<ContractDTO>();
        });

        var contractInMemory = contractsInMemory
            .FirstOrDefault(c => c.TravelerId == travelerId && c.TourId == tourId);

        if (contractInMemory != null)
        {
            return contractInMemory.Status;
        }

        // Kiểm tra trong cơ sở dữ liệu
        var contractInDatabase = await _dbContext.BlockContracts
            .FirstOrDefaultAsync(c => c.TravelerId == travelerId && c.TourId == tourId);

        if (contractInDatabase != null)
        {
            return contractInDatabase.Status;
        }

        throw new Exception("Không tìm thấy hợp đồng.");
    }


    public async Task<List<string>> GetLocationsByTravelerIdAsync(int travelerId)
    {
        // Lấy danh sách Location từ cơ sở dữ liệu theo TravelerId
        var locationsFromDb = await _dbContext.BlockContracts
            .Where(c => c.TravelerId == travelerId)
            .Select(c => c.Location)
            .Distinct()
            .ToListAsync();

        //// Lấy danh sách Location từ bộ nhớ nếu trạng thái là Created
        //var contractsInMemory = _memoryCache.GetOrCreate(ContractsCacheKey, entry =>
        //{
        //    entry.SlidingExpiration = TimeSpan.FromMinutes(30);
        //    return new List<ContractDTO>();
        //});

        //var locationsFromMemory = contractsInMemory
        //    .Where(c => c.TravelerId == travelerId && c.Status == "Created")
        //    .Select(c => c.Location)
        //    .Distinct()
        //    .ToList();

        //// Kết hợp dữ liệu từ bộ nhớ và cơ sở dữ liệu, loại bỏ trùng lặp
        //var allLocations = locationsFromDb
        //    .Union(locationsFromMemory)
        //    .Distinct()
        //    .ToList();

        return locationsFromDb;
    }

    public async Task<List<Location>> GetTopLocationsDetailsAsync(int top)
    {
        //// Lấy danh sách Location từ cơ sở dữ liệu, nhóm và đếm số lượng, sau đó sắp xếp
        //var topLocations = await _dbContext.BlockContracts
        //    .GroupBy(c => c.Location)
        //    .Select(g => new
        //    {
        //        Location = g.Key,
        //        Count = g.Count()
        //    })
        //    .OrderByDescending(l => l.Count) // Sắp xếp theo số lượng giảm dần
        //    .Take(top) // Lấy 8 địa điểm xuất hiện nhiều nhất
        //    .Select(l => l.Location)
        //    .ToListAsync();

        //return topLocations;
        // Lấy top 8 LocationName từ bảng BlockContracts
        var topLocationNames = await _dbContext.BlockContracts
            .GroupBy(c => c.Location)
            .Select(g => new
            {
                LocationName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(l => l.Count) // Sắp xếp theo số lượng giảm dần
            .Take(top) // Lấy 8 Location xuất hiện nhiều nhất
            .Select(l => l.LocationName)
            .ToListAsync();

        // Lấy danh sách Location chi tiết từ bảng Locations
        var detailedLocations = await _dbContext.Locations
            .Where(loc => topLocationNames.Contains(loc.LocationName))
            .ToListAsync();

        return detailedLocations;
    }


}
//}
