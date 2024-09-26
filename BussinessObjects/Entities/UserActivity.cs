using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class UserActivity
    {
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity? Activity { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
