using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Entities
{
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractId { get; set; }

        public int CreatedById { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        // Khóa ngoại cho người thanh toán hợp đồng
        public int PaidById { get; set; }
        public ApplicationUser? PaidByUser { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public string Location { get; set; }

        public string? PriceDetails { get; set; }


    }

}
