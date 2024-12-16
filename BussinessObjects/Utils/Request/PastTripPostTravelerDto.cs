using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Utils.Request
{
    public class PastTripPostTravelerDto
    {
        public string? TourId { get; set; }
        public int TravelerId { get; set; }
        public string? Caption { get; set; }

        [Range(1, 5, ErrorMessage = "Star value must be between 1 and 5.")]
        public int? Star { get; set; }
        public List<string>? TripImages { get; set; }
    }
}
