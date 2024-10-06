using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }
        public int TravelerId { get; set; }
        
        public int LocalId { get; set; }

        public int Rate { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        //public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
