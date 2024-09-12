﻿using System;
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
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }

        public string ImageUser { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
