using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class HomePhoto
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhotoId { get; set; }
        public int UserId { get; set; }

        // Photo URL
        public string HomePhotoUrl { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

    }
}
