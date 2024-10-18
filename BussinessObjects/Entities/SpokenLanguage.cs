namespace BussinessObjects.Entities
{
    public class SpokenLanguage
    {
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int LanguageId { get; set; }
        public Language? Language { get; set; }
        public string? Proficiency { get; set; }
    }
}
