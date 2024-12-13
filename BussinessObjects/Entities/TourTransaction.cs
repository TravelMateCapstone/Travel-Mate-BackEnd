namespace BusinessObjects.Entities
{
    public class TourTransaction
    {
        public string? TourId { get; set; }
        public string? TourName { get; set; }
        public int localId { get; set; }
        public string? LocalName { get; set; }
        public int? TravelerId { get; set; }
        public string? TravelerName { get; set; }
        public DateTime TransactionTime { get; set; }
        public double? Price { get; set; }
    }
}
