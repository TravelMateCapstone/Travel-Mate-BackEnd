namespace BussinessObjects.Entities
{
    public class UserEducation
    {
        public int UniversityId { get; set; }
        public University? University { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? GraduationYear { get; set; }
    }
}
