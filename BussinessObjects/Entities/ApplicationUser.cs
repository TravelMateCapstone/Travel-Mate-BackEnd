using Microsoft.AspNetCore.Identity;

namespace BusinessObjects.Entities
{
    /// <summary>
    /// IdentityUser<int> tức là cấu hình cho userid là int nếu muốn string thì thay bằng string
    /// Trong identity đã có sẳn những prop phổ biến như: email, phone, password, ...
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public DateTime RegistrationTime { get; set; } // Thời gian đăng ký
        public int? MatchingActivitiesCount { get; set; }
        public Profile Profiles { get; set; }
        public CCCD CCCDs { get; set; }
        public UserHome UserHome { get; set; }
        public ICollection<Friendship>? Friendships { get; set; }
        public ICollection<UserActivity>? UserActivities { get; set; }
        public ICollection<UserLocation>? UserLocations { get; set; }
        public ICollection<EventParticipants>? EventParticipants { get; set; }
        // Quan hệ với bảng Friendship: Người dùng gửi lời mời kết bạn
        public ICollection<Friendship>? SentFriendRequests { get; set; }

        // Quan hệ với bảng Friendship: Người dùng nhận lời mời kết bạn
        public ICollection<Friendship>? ReceivedFriendRequests { get; set; }
        //public ICollection<Feedback>? Feedbacks { get; set; }

        // Liên kết với các bảng khác
        public ICollection<GroupParticipant>? GroupParticipants { get; set; }
        public ICollection<PastTripPost>? PastTripPosts { get; set; }
        public ICollection<PastTripPost>? PastTripPostReviews { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<SpokenLanguages>? SpokenLanguages { get; set; }
        public ICollection<Request>? Requests { get; set; }
        public ICollection<Request>? ReceivedRequests { get; set; }
        public ICollection<Contract>? CreatedContracts { get; set; }
        public ICollection<Contract>? PaidContracts { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }
        public ICollection<Report>? Reports { get; set; }
        public ICollection<Report>? ReceivedReports { get; set; }
        public UserDescription? UserDescription { get; set; }
       
        public DetailForm? DetailForm { get; set; }
        public ICollection<OnTravelling>? OnTravel { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
        public ICollection<UserEducation>? UserEducations { get; set; }
        public ICollection<GroupPost>? GroupPosts { get; set; }
        public ICollection<Group>? Groups { get; set; }
        public ICollection<PostComment>? PostComments { get; set; }
    }
}
