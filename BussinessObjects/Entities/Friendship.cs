using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.EnumClass;

namespace BusinessObjects.Entities
{
    public class Friendship
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendshipId { get; set; }
        // Người dùng gửi lời mời kết bạn
        public int UserId1 { get; set; }
        [ForeignKey("UserId1")]
        public virtual ApplicationUser? User1 { get; set; }

        // Người dùng nhận lời mời kết bạn
        public int UserId2 { get; set; }
        [ForeignKey("UserId2")]
        public virtual ApplicationUser? User2 { get; set; }

        // Trạng thái của mối quan hệ (Pending, Accepted, Rejected)
        [Required]
        public FriendshipStatus Status { get; set; }

        // Thời gian tạo yêu cầu kết bạn
        public DateTime CreatedAt { get; set; }

        // Thời gian chấp nhận yêu cầu kết bạn
        public DateTime? ConfirmedAt { get; set; }
    }
}
