using BusinessObjects.Entities;
using BusinessObjects;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace TravelMateAPI.Services.ProfileService
{
    public class CheckProfileService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITourRepository _tourRepository;
        private readonly IContractService _contractService;
        private readonly TourDAO _tourDAO;

        public CheckProfileService(ApplicationDBContext context, UserManager<ApplicationUser> userManager, ITourRepository tourRepository, IContractService contractService, TourDAO tourDAO)
        {
            _context = context;
            _userManager = userManager;
            _tourRepository = tourRepository;
            _contractService = contractService;
            _tourDAO = tourDAO;
        }

        public async Task<IActionResult> CheckProfileCompletion(int userId)
        {
            try
            {
                var totalPercentage = 0;
                var incompleteModels = new List<string>();

                // Kiểm tra Profile
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
                if (profile != null)
                {
                    //// 2 trường description 

                    //!string.IsNullOrEmpty(profile.Description) &&
                    //!string.IsNullOrEmpty(profile.WhyUseTravelMate) &&
                    //!string.IsNullOrEmpty(profile.MusicMoviesBooks&&
                    //!string.IsNullOrEmpty(profile.Address)
                    if (!string.IsNullOrEmpty(profile.Phone))
                    {
                        totalPercentage += 18;
                    }
                    else
                    {
                        incompleteModels.Add("Số Điện Thoại");
                    }
                }
                else
                {
                    incompleteModels.Add("Số Điện Thoại");
                }

                // Kiểm tra UserHome
                var userHome = await _context.UserHomes.FirstOrDefaultAsync(uh => uh.UserId == userId);
                if (userHome != null)
                {
                    //!string.IsNullOrEmpty(userHome.RoomType) &&
                    if (userHome.MaxGuests > 0 &&
                        !string.IsNullOrEmpty(userHome.GuestPreferences) &&
                        !string.IsNullOrEmpty(userHome.AllowedSmoking) &&
                        !string.IsNullOrEmpty(userHome.RoomDescription) &&
                        !string.IsNullOrEmpty(userHome.RoomMateInfo) &&
                        !string.IsNullOrEmpty(userHome.Amenities) &&
                        !string.IsNullOrEmpty(userHome.Transportation) &&
                        !string.IsNullOrEmpty(userHome.OverallDescription))
                    {
                        totalPercentage += 5;
                    }
                    else
                    {
                        incompleteModels.Add("Nhà của bạn");
                    }
                }
                else
                {
                    incompleteModels.Add("Nhà của bạn");
                }

                // Kiểm tra CCCD
                var cccd = await _context.CCCDs.FirstOrDefaultAsync(c => c.UserId == userId);
                if (cccd != null)
                {
                    if (!string.IsNullOrEmpty(cccd.imageFront) &&
                        !string.IsNullOrEmpty(cccd.imageBack))
                    {
                        totalPercentage += 18;
                    }
                    else
                    {
                        incompleteModels.Add("CCCD (Mặt trước & Mặt sau)");
                    }

                    if (!string.IsNullOrEmpty(cccd.PublicSignature))
                    {
                        totalPercentage += 18;
                    }
                    else
                    {
                        incompleteModels.Add("Chữ Ký Số");
                    }
                }
                else
                {
                    incompleteModels.Add("CCCD & Chữ Ký Số");
                }

                // Kiểm tra UserActivity
                var userActivity = await _context.UserActivities.AnyAsync(ua => ua.UserId == userId);
                if (userActivity)
                {
                    totalPercentage += 10;
                }
                else
                {
                    incompleteModels.Add("Hoạt động yêu thích");
                }

                // Kiểm tra UserLocation
                var userLocation = await _context.UserLocations.AnyAsync(ul => ul.UserId == userId);
                if (userLocation)
                {
                    totalPercentage += 15;
                }
                else
                {
                    incompleteModels.Add("Địa phương đăng ký");
                }

                // Kiểm tra UserEducation
                var userEducation = await _context.UserEducations.AnyAsync(ue => ue.UserId == userId);
                if (userEducation)
                {
                    totalPercentage += 2;
                }
                else
                {
                    incompleteModels.Add("Học vấn");
                }

                // Kiểm tra SpokenLanguage
                var spokenLanguage = await _context.SpokenLanguages.AnyAsync(sl => sl.UserId == userId);
                if (spokenLanguage)
                {
                    totalPercentage += 2;
                }
                else
                {
                    incompleteModels.Add("Ngôn ngữ");
                }

                // Kiểm tra UserContact
                var userContact = await _context.UserContacts.FirstOrDefaultAsync(uh => uh.UserId == userId);
                if (userContact != null)
                {
                    //!string.IsNullOrEmpty(userHome.RoomType) &&
                    if (!string.IsNullOrEmpty(userContact.Name) &&
                        !string.IsNullOrEmpty(userContact.Phone) &&
                        !string.IsNullOrEmpty(userContact.Email) &&
                        !string.IsNullOrEmpty(userContact.NoteContact))
                    {
                        totalPercentage += 6;
                    }
                    else
                    {
                        incompleteModels.Add("Liên hệ khẩn cấp");
                    }
                    //totalPercentage += 6;
                }
                else
                {
                    incompleteModels.Add("Liên hệ khẩn cấp");
                }

                // Kiểm tra UserBank
                var userBank = await _context.UserBanks.FirstOrDefaultAsync(uh => uh.UserId == userId);
                if (userBank != null)
                {
                    //!string.IsNullOrEmpty(userHome.RoomType) &&
                    if (!string.IsNullOrEmpty(userBank.BankName) &&
                        !string.IsNullOrEmpty(userBank.BankNumber) &&
                        !string.IsNullOrEmpty(userBank.OwnerName))
                    {
                        totalPercentage += 6;
                    }
                    else
                    {
                        incompleteModels.Add("Thông tin ngân hàng");
                    }
                    //totalPercentage += 6;
                }
                else
                {
                    incompleteModels.Add("Thông tin ngân hàng");
                }

                return new OkObjectResult(new
                {
                    TotalPercentage = totalPercentage,
                    IncompleteModels = incompleteModels
                });

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Error = ex.Message });
            }
        }

    }
}
