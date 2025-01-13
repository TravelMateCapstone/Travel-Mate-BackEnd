using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using BusinessObjects.Utils.Response;

namespace TravelMateAPI.Models
{
    public class AutoMapperProfile : AutoMapper.Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<UserActivity, UserActivityDTO>()
             .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.Activity.ActivityName));

            CreateMap<UserLocation, UserLocationDTO>()
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.LocationName));

            CreateMap<Group, GroupDTO>();

            CreateMap<GroupPost, GroupPostDTO>()
                .ForMember(dest => dest.PostCreatorAvatar, opt => opt.MapFrom(src => src.PostBy.Profiles.ImageUser))
                .ForMember(dest => dest.PostCreatorName, opt => opt.MapFrom(src => src.PostBy.FullName));

            CreateMap<PostComment, PostCommentDTO>()
                .ForMember(dest => dest.Commentor, opt => opt.MapFrom(src => src.CommentedBy.FullName))
                .ForMember(dest => dest.CommentorAvatar, opt => opt.MapFrom(src => src.CommentedBy.Profiles.ImageUser));


            CreateMap<GroupParticipant, GroupMemberDTO>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.MemberAvatar, opt => opt.MapFrom(src => src.User.Profiles.ImageUser))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.User.Profiles.City));

            CreateMap<ApplicationUser, UserInformationDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.UserAvatarUrl, opt => opt.MapFrom(src => src.Profiles.ImageUser))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Profiles.City));

            CreateMap<TourDto, Tour>();
            CreateMap<Tour, TourDto>();

            CreateMap<Tour, TourBriefDto>();

            CreateMap<ApplicationUser, Participants>()
             .ForMember(dest => dest.ParticipantId, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Profiles.Gender))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Profiles.Address))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Profiles.Phone));

            CreateMap<PastTripPost, PastTripPostTravelerDto>();
            CreateMap<PastTripPostTravelerDto, PastTripPost>();

            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Profiles.City))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Profiles.ImageUser));

        }

    }
}
