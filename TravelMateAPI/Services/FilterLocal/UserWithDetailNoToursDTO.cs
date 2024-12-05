namespace TravelMateAPI.Services.FilterLocal
{
    public class UserWithDetailNoToursDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ProfileDTO2 Profile { get; set; }
        public List<string> Roles { get; set; }
        public double Star { get; set; }
        public int CountConnect { get; set; }
        public CCCDDTO2 CCCD { get; set; }
       
        public List<int> ActivityIds { get; set; }
        //public int SimilarityScore { get; set; } // Điểm tương tự
        public List<int> LocationIds { get; set; }
    }   

    public class ProfileDTO2
    {
        public int ProfileId { get; set; }
        public string? Address { get; set; }
        public string? ImageUser { get; set; }
    }

    public class CCCDDTO2
    {
        public string? Dob { get; set; }
        public string? Sex { get; set; }
        public int Age { get; set; }  // Thêm tuổi
    }

    

}
