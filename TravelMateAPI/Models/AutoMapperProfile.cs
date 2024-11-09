using BusinessObjects.Entities;
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
    .ForMember(dest => dest.PostCreatorAvatar, opt => opt.MapFrom(src =>
        src.PostBy != null && src.PostBy.Profiles != null && src.PostBy.Profiles.ImageUser != null
            ? src.PostBy.Profiles.ImageUser
            : "")) // Use empty string if null
    .ForMember(dest => dest.PostCreatorName, opt => opt.MapFrom(src =>
        src.PostBy != null && src.PostBy.FullName != null
            ? src.PostBy.FullName
            : "")); // Use empty string if null

            CreateMap<PostComment, PostCommentDTO>()
                .ForMember(dest => dest.Commentor, opt => opt.MapFrom(src =>
                    src.CommentedBy != null && src.CommentedBy.FullName != null
                        ? src.CommentedBy.FullName
                        : "")) // Use empty string if null
                .ForMember(dest => dest.CommentorAvatar, opt => opt.MapFrom(src =>
                    src.CommentedBy != null && src.CommentedBy.Profiles != null && src.CommentedBy.Profiles.ImageUser != null
                        ? src.CommentedBy.Profiles.ImageUser
                        : "")); // Use empty string if null

            CreateMap<GroupParticipant, GroupMemberDTO>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src =>
                    src.User != null && src.User.FullName != null
                        ? src.User.FullName
                        : "")) // Use empty string if null
                .ForMember(dest => dest.MemberAvatar, opt => opt.MapFrom(src =>
                    src.User != null && src.User.Profiles != null && src.User.Profiles.ImageUser != null
                        ? src.User.Profiles.ImageUser
                        : "")) // Use empty string if null
                .ForMember(dest => dest.City, opt => opt.MapFrom(src =>
                    src.User != null && src.User.Profiles != null && src.User.Profiles.City != null
                        ? src.User.Profiles.City
                        : "")); // Use empty string if null

        }
    }
}
