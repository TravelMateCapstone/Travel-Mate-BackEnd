using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    /// <summary>
    /// IdentityUser<int> tức là cấu hình cho userid là int nếu muốn string thì thay bằng string
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public DateTime RegistrationTime { get; set; } // Thời gian đăng ký
        public int? MatchingActivitiesCount { get; set; }
        public ICollection<Profile>? Profiles { get; set; }
        //public virtual Profile? Profiles { get; set; }
        public ICollection<Friendship>? Friendships { get; set; }
        public ICollection<UserActivity>? UserActivities { get; set; }
        public ICollection<UserLocation>? UserLocations { get; set; }
        public ICollection<EventParticipants>?  EventParticipants { get; set; }
        // Quan hệ với bảng Friendship: Người dùng gửi lời mời kết bạn
        public ICollection<Friendship>? SentFriendRequests { get; set; }

        // Quan hệ với bảng Friendship: Người dùng nhận lời mời kết bạn
        public ICollection<Friendship>? ReceivedFriendRequests { get; set; }
        //public ICollection<Feedback>? Feedbacks { get; set; }
    }
}
