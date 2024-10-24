using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }

        public int RequestById { get; set; }
        public ApplicationUser? RequestByUser { get; set; }

        public int RequestToId { get; set; }
        public ApplicationUser? RequestToUser { get; set; }

        public DateTime RequestTime { get; set; } = DateTime.UtcNow;

        public string Status { get; set; }

        public int FormVersion { get; set; }
    }
}
