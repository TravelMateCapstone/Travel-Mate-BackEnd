using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class PastTripPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PastTripPostId { get; set; }

        public int TravelerId { get; set; }

        public ApplicationUser? Traveler { get; set; }

        public bool IsPublic { get; set; }

        public bool IsCaptionEdit { get; set; }

        public string? Caption { get; set; }

        public int LocalId { get; set; }
        public ApplicationUser? Local { get; set; }
        public string? Review { get; set; }

        public bool IsReviewEdited { get; set; }

        [Range(1, 5)]
        public int Star { get; set; }

        public string Location { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<PostPhoto>? PostPhotos { get; set; }
    }
}
