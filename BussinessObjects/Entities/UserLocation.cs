using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class UserLocation
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
