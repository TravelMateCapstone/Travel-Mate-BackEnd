using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class EventParticipants
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int EventParticipantId { get; set; }

        [Key, Column(Order = 0)]
        public int EventId { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        
        public DateTime JoinedAt { get; set; }
        public bool? Notification { get; set; }

        // Navigation property to link with Event
        public virtual Event? Event { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }  
    }
}
