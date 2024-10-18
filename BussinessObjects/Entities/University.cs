using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Entities
{
    public class University
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UniversityId { get; set; }

        public string UniversityName { get; set; }
        public ICollection<UserEducation>? Users { get; set; }
    }
}
