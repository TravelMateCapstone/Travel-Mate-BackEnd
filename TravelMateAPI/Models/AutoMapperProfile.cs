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
            //.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName));

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

            CreateMap<PastTripPost, PastTripPostDTO>()
           .ForMember(dest => dest.TravelerName, opt => opt.MapFrom(src => src.Traveler.FullName))
           .ForMember(dest => dest.TravelerAvatar, opt => opt.MapFrom(src => src.Traveler.Profiles.ImageUser))
           .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.Local.FullName))
           .ForMember(dest => dest.LocalAvatar, opt => opt.MapFrom(src => src.Local.Profiles.ImageUser));

            CreateMap<PastTripPostInputDTO, PastTripPost>();

            CreateMap<TravelerPastTripPostDTO, PastTripPost>();

            CreateMap<LocalPastTripPostDTO, PastTripPost>();

            CreateMap<PostPhotoInputDTO, PostPhoto>();

            CreateMap<LocalExtraDetailForm, TravelerExtraDetailForm>()
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services));
        }

    }
}
