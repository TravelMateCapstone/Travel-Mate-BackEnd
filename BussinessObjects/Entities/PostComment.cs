using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObjects.Entities
{
    public class PostComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public int CommentedById { get; set; }
        [JsonIgnore]
        public ApplicationUser? CommentedBy { get; set; }
        public int PostId { get; set; }
        [JsonIgnore]
        public GroupPost? Post { get; set; }
        public bool IsEdited { get; set; }
        [Required]
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
