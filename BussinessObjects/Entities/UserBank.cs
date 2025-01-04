using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class UserBank
    {
        [Key]
        public int UserId { get; set; }

        public string? BankName { get; set; }

        public string? BankNumber { get; set; }

        public string? OwnerName { get; set; }

    }
}
