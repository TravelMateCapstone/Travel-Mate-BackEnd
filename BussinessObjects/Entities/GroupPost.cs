using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Entities
{
    public class GroupPost
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public int PostById { get; set; }

        public ApplicationUser? PostByUser { get; set; }

        public int GroupId { get; set; }

        public Group? Group { get; set; }

        public ICollection<PostPhoto>? PostPhotos { get; set; }
        public virtual ICollection<PostComment>? Comments { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
    }

}
