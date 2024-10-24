using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class PastTripPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public int UserId { get; set; }

        public ApplicationUser? UserPastTrip { get; set; }

        public string Privacy { get; set; }

        public string? Title { get; set; }

        public string? Caption { get; set; }

        public int ReviewById { get; set; }
        public ApplicationUser? ReviewByUser { get; set; }
        public string? Review { get; set; }

        //[StringLength(30)]

        //public decimal Longitude { get; set; }
        //[StringLength(30)]
        //public decimal Latitude { get; set; }

        [Range(1, 5)]
        public int Star { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PostPhoto>? Photos { get; set; }
    }
}
