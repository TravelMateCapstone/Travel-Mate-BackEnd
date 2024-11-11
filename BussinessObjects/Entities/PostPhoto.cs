using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObjects.Entities
{
    public class PostPhoto
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostPhotoId { get; set; }
        public string PhotoUrl { get; set; }

        public int? PastTripPostId { get; set; }
        [JsonIgnore]
        public PastTripPost? PastTripPost { get; set; }
    }
}
