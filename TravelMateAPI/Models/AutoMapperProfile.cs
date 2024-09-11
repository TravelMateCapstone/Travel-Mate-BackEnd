using BussinessObjects.Entities;
using DataAccess;

namespace TravelMateAPI.Models
{
    public class AutoMapperProfile
    {
        public AutoMapperProfile()
        {
            // Ánh xạ giữa ApplicationUser và ApplicationUserDTO
            //CreateMap<ApplicationUser, ApplicationUserDTO>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
