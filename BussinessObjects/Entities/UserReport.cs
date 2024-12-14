using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class UserReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserReportId { get; set; }

        public int UserId { get; set; }

        public string Detail { get; set; }

        public string ImageReport {  get; set; }

        public string ReportType { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }
    }
}
