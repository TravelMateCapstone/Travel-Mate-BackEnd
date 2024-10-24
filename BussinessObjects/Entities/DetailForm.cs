using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Entities
{
    public class DetailForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormId { get; set; }
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCurrent { get; set; }
        public string? Description { get; set; }
    }
}
