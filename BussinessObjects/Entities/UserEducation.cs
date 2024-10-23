using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class UserEducation
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int UserActivityId { get; set; }
        [Key, Column(Order = 0)]
        public int UniversityId { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        public DateTime GraduationYear { get; set; }
        public virtual University? University { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
