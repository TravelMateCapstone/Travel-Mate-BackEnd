using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class UserActivity
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int ActivityId { get; set; }
        public virtual Activity? Activity { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
