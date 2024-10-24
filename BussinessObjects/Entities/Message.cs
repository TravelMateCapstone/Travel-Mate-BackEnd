using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public int CreatedById { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        public int SendToId { get; set; }
        public ApplicationUser? SendToUser { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
