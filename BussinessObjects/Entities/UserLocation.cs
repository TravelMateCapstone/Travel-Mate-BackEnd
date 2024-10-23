using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class UserLocation
    {
        

        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int LocationId { get; set; }

        public virtual Location? Location { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
