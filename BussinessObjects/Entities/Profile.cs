using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class Profile
    {
        [Key]
        public string UserId { get; set; }
        
        public string FullName { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }

        public string? ImageUser { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
