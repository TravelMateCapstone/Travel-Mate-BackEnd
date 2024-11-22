using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }
        public int ReceiverId { get; set; }
        public ApplicationUser? Receiver { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
