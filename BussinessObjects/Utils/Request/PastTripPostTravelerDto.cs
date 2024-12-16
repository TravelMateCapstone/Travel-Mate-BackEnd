namespace BusinessObjects.Utils.Request
{
    public class PastTripPostTravelerDto
    {
        public string? TourId { get; set; }
        public int TravelerId { get; set; }
        public string? Caption { get; set; }
        public int? Star { get; set; }
        public List<string>? TripImages { get; set; }
    }
}
