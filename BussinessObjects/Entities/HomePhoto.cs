using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Entities
{
    public class HomePhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }

        public int UserId { get; set; }

        public UserHome? UserHome { get; set; }
    }
}
