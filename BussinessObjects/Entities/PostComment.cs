using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class PostComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public int CommentedById { get; set; }
        public ApplicationUser? CommentedByUser { get; set; }
        public int PostId { get; set; }
        public GroupPost? GroupPost { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.UtcNow;
    }
}
