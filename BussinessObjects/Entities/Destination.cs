using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class Destination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DestinationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<OnTravelling>? Travellers { get; set; }
    }
}
