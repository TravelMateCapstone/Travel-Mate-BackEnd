using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Repositories.Interface;
using TravelMateAPI.Services.FilterLocal;

namespace TravelMateAPI.Services.FilterTour
{
    public class FilterTourService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITourRepository _tourRepository;
        private readonly IContractService _contractService;
        private readonly TourDAO _tourDAO;
        private readonly IMongoCollection<Tour> _mongoContext;
        private readonly FilterUserService _filterService;

        public FilterTourService(ApplicationDBContext context, UserManager<ApplicationUser> userManager, ITourRepository tourRepository, IContractService contractService, TourDAO tourDAO, MongoDbContext mongoContext, FilterUserService filterService)
        {
            _context = context;
            _userManager = userManager;
            _tourRepository = tourRepository;
            _contractService = contractService;
            _tourDAO = tourDAO;
            _mongoContext = mongoContext.GetCollection<Tour>("Tours");
            _filterService = filterService;
        }

        public async Task<List<TourWithUserDetailsDTO>> GetAllTourBriefWithUserDetailsAsync()
        {
            // Lấy toàn bộ tour với trạng thái được chấp nhận
            var tours = await _mongoContext
                .Find(t => t.ApprovalStatus == ApprovalStatus.Accepted)
                .ToListAsync();

            // Lấy danh sách các LocalId từ các tour
            var localIds = tours.Select(t => t.Creator.Id).Distinct().ToList();

            // Lấy thông tin UserWithDetailsDTO của các LocalId
            var users = await _filterService.GetAllUsersWithDetailsByIdsAsync(localIds);

            // Ánh xạ Tour với User
            var result = tours.Select(t => new TourWithUserDetailsDTO
            {
                TourId = t.TourId,
                LocalId = t.Creator.Id,
                //RegisteredGuests = t.TourSchedule.Participants.Count,
                MaxGuests = t.MaxGuests,
                Location = t.Location,
                StartDates = t.Schedules?.Select(s => s.StartDate).ToList() ?? new List<DateTime>(), // Lấy danh sách ngày khởi hành
                //EndDate = t.TourSchedule.EndDate,
                TourDescription = t.TourDescription,
                NumberOfDays = t.NumberOfDays,
                //NumberOfDays = (t.TourSchedule.EndDate - t.TourSchedule.StartDate).Days,
                //NumberOfNights = (t.TourSchedule.EndDate - t.TourSchedule.StartDate).Days - 1,
                TourName = t.TourName,
                Price = t.Price,
                TourImage = t.TourImage,
                User = users.FirstOrDefault(u => u.UserId == t.Creator.Id)
            }).ToList();

            return result;
        }

        public async Task<List<TourWithUserDetailsDTO>> GetAllTourBriefWithUserDetailsByLocationAsync(string location)
        {
            // Lấy toàn bộ tour với trạng thái được chấp nhận
            ////Kiểm tra nếu danh sách Schedules không null và có ít nhất một ngày khởi hành (StartDate) nhỏ hơn hoặc bằng ngày hiện tại.
            //var tours = await _mongoContext
            //    .Find(t => t.ApprovalStatus == ApprovalStatus.Accepted && t.Location == location)
            //    .ToListAsync();

            // Lấy thời gian hiện tại theo múi giờ Việt Nam
            var currentDateTime = GetTimeZone.GetVNTimeZoneNow();

            //// Lấy toàn bộ tour với trạng thái được chấp nhận và ngày bắt đầu >= thời điểm hiện tại
            //var tours = await _mongoContext
            //    .Find(t => t.ApprovalStatus == ApprovalStatus.Accepted
            //               && t.Location == location
            //               && t.StartDate >= currentDateTime)
            //    .ToListAsync();


            //Kiểm tra nếu danh sách Schedules không null và có ít nhất một ngày khởi hành (StartDate) nhỏ hơn hoặc bằng ngày hiện tại.
            var tours = await _mongoContext
                .Find(t => t.ApprovalStatus == ApprovalStatus.Accepted && t.Location == location &&
                   t.Schedules != null &&
                   t.Schedules.Any(s => s.StartDate <= currentDateTime))
                .ToListAsync();

            // Lấy danh sách các LocalId từ các tour
            var localIds = tours.Select(t => t.Creator.Id).Distinct().ToList();

            // Lấy thông tin UserWithDetailsDTO của các LocalId
            var users = await _filterService.GetAllUsersWithDetailsByIdsAsync(localIds);

            // Ánh xạ Tour với User
            var result = tours.Select(t => new TourWithUserDetailsDTO
            {
                //TourId = t.TourId,
                //LocalId = t.Creator.Id,
                ////RegisteredGuests = t.Participants.Count,
                //MaxGuests = t.MaxGuests,
                //Location = t.Location,
                //StartDate = t.StartDate,
                //EndDate = t.EndDate,
                //TourDescription = t.TourDescription,
                //NumberOfDays = (t.EndDate - t.StartDate).Days,
                //NumberOfNights = (t.EndDate - t.StartDate).Days - 1,
                //TourName = t.TourName,
                //Price = t.Price,
                //TourImage = t.TourImage,
                //User = users.FirstOrDefault(u => u.UserId == t.Creator.Id)
                TourId = t.TourId,
                LocalId = t.Creator.Id,
                //RegisteredGuests = t.TourSchedule.Participants.Count,
                MaxGuests = t.MaxGuests,
                Location = t.Location,
                StartDates = t.Schedules?.Select(s => s.StartDate).ToList() ?? new List<DateTime>(), // Lấy danh sách ngày khởi hành
                //EndDate = t.TourSchedule.EndDate,
                TourDescription = t.TourDescription,
                NumberOfDays = t.NumberOfDays,
                NumberOfNights = t.NumberOfDays - 1 ,
                //NumberOfDays = (t.TourSchedule.EndDate - t.TourSchedule.StartDate).Days ,
                //NumberOfNights = (t.TourSchedule.EndDate - t.TourSchedule.StartDate).Days - 1,
                TourName = t.TourName,
                Price = t.Price,
                TourImage = t.TourImage,
                User = users.FirstOrDefault(u => u.UserId == t.Creator.Id)
            }).ToList();


            return result;
        }
    }
}
