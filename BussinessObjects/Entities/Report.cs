using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        public int SentById { get; set; }
        public ApplicationUser? SentByUser { get; set; }

        public int ReportToId { get; set; }
        public ApplicationUser? ReportToUser { get; set; }

        public string ReportType { get; set; }

        public DateTime ReportTime { get; set; } = DateTime.UtcNow;

        public string Status { get; set; }
    }
}
