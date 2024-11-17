using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Entities
{
    public class DetailForm
    {
        [Key]
        public int DetailFormId { get; set; }

        public int LocalId { get; set; }
        public ApplicationUser? Local { get; set; }

        public int? TravelerId { get; set; }
        public ICollection<ApplicationUser>? Travelers { get; set; }

        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
