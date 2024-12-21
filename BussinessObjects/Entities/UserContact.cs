using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class UserContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserContactId { get; set; }

        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Phone{ get; set; }

        public string? Email { get; set; }

        public string? NoteContact { get; set; }

    }
}
