using BusinessObjects.Entities;

namespace BusinessObjects.Utils.Request
{
    public class TourBriefDto
    {
        public string TourId { get; set; }
        public CreatorInfo? Creator { get; set; }
        public int RegisteredGuests { get; set; } = 0;
        public int MaxGuests { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfNights { get; set; }
        public string TourName { get; set; }
        public double? Price { get; set; }
        public string TourImage { get; set; }
    }
}
