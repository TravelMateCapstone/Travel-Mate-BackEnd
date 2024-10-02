using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Entities
{
    public class Friend
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ContactUserId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
