using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Entities
{
    public class UserEducation
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int UserActivityId { get; set; }
        [Key, Column(Order = 0)]
        public int UniversityId { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        public DateTime GraduationYear { get; set; }
        public virtual University? University { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
