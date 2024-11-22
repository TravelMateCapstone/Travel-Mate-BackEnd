using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class GroupPost
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupPostId { get; set; }

        public string? Title { get; set; }

        public DateTime CreatedTime { get; set; }

        public int? PostById { get; set; }

        public ApplicationUser? PostBy { get; set; }

        public int? GroupId { get; set; }

        public Group? Group { get; set; }

        public ICollection<GroupPostPhoto>? GroupPostPhotos { get; set; }
        public ICollection<PostComment>? PostComments { get; set; }
        //public ICollection<Reaction>? Reactions { get; set; }
    }

}
