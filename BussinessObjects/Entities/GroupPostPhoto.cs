using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObjects.Entities
{
    public class GroupPostPhoto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupPostPhotoId { get; set; }
        public string PhotoUrl { get; set; }

        [JsonIgnore]
        public int? PostId { get; set; }
        [JsonIgnore]
        public GroupPost? Post { get; set; }
    }
}
