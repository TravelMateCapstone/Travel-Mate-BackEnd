using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using System.ComponentModel.DataAnnotations;
using BusinessObjects.Utils.Response;

namespace TravelMateAPI.Services.FilterLocal
{
    public class UserWithDetailsDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ProfileDTO Profile { get; set; }
        public List<string> Roles { get; set; }
        public double Star { get; set; }
        public int CountConnect { get; set; }
        public CCCDDTO CCCD { get; set; }
        //public UserActivitiesDTO UserActivities { get; set; }
        public List<int> ActivityIds { get; set; }
        //public int SimilarityScore { get; set; } // Điểm tương tự
        public List<int> LocationIds { get; set; }
        public List<TourDTO> Tours { get; set; } // Danh sách tour
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

    //public class TourDTO
    //{
    //    public string TourId { get; set; }
    //    public int LocalId { get; set; }
    //    public int RegisteredGuests { get; set; } 
    //    public int MaxGuests { get; set; }
    //    public string Location { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public int NumberOfDays { get; set; }
    //    public int NumberOfNights { get; set; }
    //    public string TourName { get; set; }
    //    public double? Price { get; set; }
    //    public string TourImage { get; set; }
    //}


}
