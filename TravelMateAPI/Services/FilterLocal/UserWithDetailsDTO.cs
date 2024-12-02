using System.ComponentModel.DataAnnotations;

namespace TravelMateAPI.Services.FilterLocal
{
    public class UserWithDetailsDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ProfileDTO Profile { get; set; }
        public List<string> Roles { get; set; }
        public CCCDDTO CCCD { get; set; }
        //public UserActivitiesDTO UserActivities { get; set; }
        public List<int> ActivityIds { get; set; }
        //public int SimilarityScore { get; set; } // Điểm tương tự
        public List<int> LocationIds { get; set; }

    }

    public class ProfileDTO
    {
        public int ProfileId { get; set; }
        public string? Address { get; set; }
    }

    public class CCCDDTO
    {
        public string? Dob { get; set; }
        public string? Sex { get; set; }
        public int Age { get; set; }  // Thêm tuổi
    }

    public class UserActivitiesDTO
    {
        public List<int> ActivityIds { get; set; } = new List<int>();
    }



}
