using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }
        public int CreaterUserId { get; set; }
        public string EventName { get; set; }
        public string? Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string EventLocation { get; set; }

        
        public ICollection<EventParticipants>? EventParticipants { get; set; }
    }
}
