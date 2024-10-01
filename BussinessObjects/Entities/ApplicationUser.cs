using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public DateTime RegistrationTime { get; set; } // Thời gian đăng ký
        public ICollection<Profile>? Profiles { get; set; }
        public ICollection<Friend>? Friends { get; set; }
        public ICollection<UserActivity>? UserActivitys { get; set; }
        public ICollection<UserLocation>? UserLocations { get; set; }
        public ICollection<EventParticipants>?  EventParticipants { get; set; }
    }
}
