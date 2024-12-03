using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class BlockContract
    {
        [Key]
        public string Id { get; set; } // ID duy nhất của hợp đồng, được tạo bằng GUID.

        public int TravelerId { get; set; } // ID của người đi du lịch.
        public int LocalId { get; set; } // ID của người địa phương.
        public int TourId { get; set; } // ID của Tour du lịch.
        public string Details { get; set; } // Chi tiết hợp đồng (ví dụ: nội dung thỏa thuận, điều khoản).
        public string Status { get; set; } // Trạng thái của hợp đồng: Created, Signed, Completed, Cancelled.
        public string TravelerSignature { get; set; } // Chữ ký số của người đi du lịch (RSA).
        public string LocalSignature { get; set; } // Chữ ký số của người địa phương (RSA).
        public DateTime CreatedAt { get; set; } // Thời điểm hợp đồng được tạo.
        public string PreviousHash { get; set; } // Hash của block (hợp đồng) trước đó trong blockchain.
        public string Hash { get; set; } // Hash của block hiện tại, được tính dựa trên dữ liệu hợp đồng.
    }
}
