using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class CCCD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CCCDId { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }
        //mặt trước
        public string? imageFront { get; set; }  //ảnh mặt trước
        public string? id { get; set; }
        public string? name { get; set; }
        public string? dob { get; set; }
        public string? sex { get; set; }
        public string? nationality { get; set; }
        public string? home { get; set; }
        public string? address { get; set; }
        public string? doe { get; set; }

        //mặt sau
        public string? imageBack { get; set; }   //ảnh mặt sau
        public string? features { get; set; }
        public string? issue_date { get; set; }
        public List<string>? mrz { get; set; }
        public string? issue_loc { get; set; }

        //chữ ký số
        public string? PublicSignature { get; set; }
    }
}
