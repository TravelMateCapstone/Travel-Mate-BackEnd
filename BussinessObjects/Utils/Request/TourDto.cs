using BusinessObjects.Entities;
using BusinessObjects.EnumClass;

namespace BusinessObjects.Utils.Request
{
    public class TourDto
    {
        public string? TourId { get; set; }
        public CreatorInfo? Creator { get; set; }
        public string TourName { get; set; }
        public string TourDescription { get; set; }
        public bool IsGlobalContract { get; set; }
        public double? Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfNights { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
        public string Location { get; set; }
        public int RegisteredGuests { get; set; }
        public int MaxGuests { get; set; }
        public string TourImage { get; set; }
        public List<Itinerary> Itinerary { get; set; }
        public List<CostDetail> CostDetails { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
