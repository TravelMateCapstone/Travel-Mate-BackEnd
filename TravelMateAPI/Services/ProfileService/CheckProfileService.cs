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
                    if (!string.IsNullOrEmpty(profile.Phone) &&
                        !string.IsNullOrEmpty(profile.Address) &&
                        !string.IsNullOrEmpty(profile.Description) &&
                        !string.IsNullOrEmpty(profile.WhyUseTravelMate) &&
                        !string.IsNullOrEmpty(profile.MusicMoviesBooks))
                    {
                        totalPercentage += 10;
                    }
                    else
                    {
                        incompleteModels.Add("Profile");
                    }
                }
                else
                {
                    incompleteModels.Add("Profile");
                }

                // Kiểm tra UserHome
                var userHome = await _context.UserHomes.FirstOrDefaultAsync(uh => uh.UserId == userId);
                if (userHome != null)
                {
                    if (userHome.MaxGuests > 0 &&
                        !string.IsNullOrEmpty(userHome.GuestPreferences) &&
                        !string.IsNullOrEmpty(userHome.AllowedSmoking) &&
                        !string.IsNullOrEmpty(userHome.RoomDescription) &&
                        !string.IsNullOrEmpty(userHome.RoomType) &&
                        !string.IsNullOrEmpty(userHome.RoomMateInfo) &&
                        !string.IsNullOrEmpty(userHome.Amenities) &&
                        !string.IsNullOrEmpty(userHome.Transportation) &&
                        !string.IsNullOrEmpty(userHome.OverallDescription))
                    {
                        totalPercentage += 10;
                    }
                    else
                    {
                        incompleteModels.Add("UserHome");
                    }
                }
                else
                {
                    incompleteModels.Add("UserHome");
                }

                // Kiểm tra CCCD
                var cccd = await _context.CCCDs.FirstOrDefaultAsync(c => c.UserId == userId);
                if (cccd != null)
                {
                    if (!string.IsNullOrEmpty(cccd.imageFront) &&
                        !string.IsNullOrEmpty(cccd.imageBack))
                    {
                        totalPercentage += 10;
                    }
                    else
                    {
                        incompleteModels.Add("CCCD (ImageFront or ImageBack)");
                    }

                    if (!string.IsNullOrEmpty(cccd.PublicSignature))
                    {
                        totalPercentage += 10;
                    }
                    else
                    {
                        incompleteModels.Add("CCCD (PublicSignature)");
                    }
                }
                else
                {
                    incompleteModels.Add("CCCD");
                }

                // Kiểm tra UserActivity
                var userActivity = await _context.UserActivities.AnyAsync(ua => ua.UserId == userId);
                if (userActivity)
                {
                    totalPercentage += 10;
                }
                else
                {
                    incompleteModels.Add("UserActivity");
                }

                // Kiểm tra UserLocation
                var userLocation = await _context.UserLocations.AnyAsync(ul => ul.UserId == userId);
                if (userLocation)
                {
                    totalPercentage += 10;
                }
                else
                {
                    incompleteModels.Add("UserLocation");
                }

                // Kiểm tra UserEducation
                var userEducation = await _context.UserEducations.AnyAsync(ue => ue.UserId == userId);
                if (userEducation)
                {
                    totalPercentage += 10;
                }
                else
                {
                    incompleteModels.Add("UserEducation");
                }

                // Kiểm tra SpokenLanguage
                var spokenLanguage = await _context.SpokenLanguages.AnyAsync(sl => sl.UserId == userId);
                if (spokenLanguage)
                {
                    totalPercentage += 10;
                }
                else
                {
                    incompleteModels.Add("SpokenLanguage");
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
