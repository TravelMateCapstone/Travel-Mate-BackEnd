using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObjects.Entities
{
    public class GroupPost
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string? Title { get; set; }

        public DateTime CreatedTime { get; set; }

        public int PostById { get; set; }

        //[JsonIgnore]
        public ApplicationUser? PostBy { get; set; }

        public int GroupId { get; set; }

        [JsonIgnore]
        public Group? Group { get; set; }

        public ICollection<GroupPostPhoto>? PostPhotos { get; set; }
        public ICollection<PostComment>? Comments { get; set; }
        [JsonIgnore]
        public ICollection<Reaction>? Reactions { get; set; }
    }

}
