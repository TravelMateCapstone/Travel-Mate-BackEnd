using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class GroupPostPhoto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupPostPhotoId { get; set; }
        public string PhotoUrl { get; set; }

        public int? PostId { get; set; }
        public GroupPost? Post { get; set; }
    }
}
