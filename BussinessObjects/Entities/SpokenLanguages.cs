using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class SpokenLanguages
    {
        [Key, Column(Order = 0)]
        public int LanguagesId { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        public string Proficiency { get; set; }
        
        public virtual Languages? Languages { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
