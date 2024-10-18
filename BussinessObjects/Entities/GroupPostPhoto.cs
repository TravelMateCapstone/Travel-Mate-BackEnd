using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Entities
{
    public class GroupPostPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public int PostId { get; set; }
        public GroupPost? Post { get; set; }
    }
}
