namespace BussinessObjects.Entities
{
    public class UserProfile
    {

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string? HostingAvailability { get; set; }

        public bool Gender { get; set; }

        public string? Location { get; set; }

        public DateTime BirthDate { get; set; }

        public string NationalID { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public ICollection<Language>? Languages { get; set; }
        public ICollection<University>? Universities { get; set; }
    }
}
