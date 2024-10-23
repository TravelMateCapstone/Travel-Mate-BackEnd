using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class Local
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocalId { get; set; }

        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string? AdressRegister { get; set; }

        public string? Description { get; set; }
        public DateTime? StartShare { get; set; }
        public DateTime? EndShare { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
