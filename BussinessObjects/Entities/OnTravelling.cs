namespace BussinessObjects.Entities
{
    public class OnTravelling
    {
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int DestinationId { get; set; }
        public Destination? Destination { get; set; }

        public bool IsTravelling { get; set; }
    }
}
