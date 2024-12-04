using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public string? Title { get; set; }   
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? MapHtml { get; set; }
    }
}
