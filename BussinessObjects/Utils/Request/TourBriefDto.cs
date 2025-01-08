using BusinessObjects.Entities;
using BusinessObjects.EnumClass;

namespace BusinessObjects.Utils.Request
{
    public class TourBriefDto
    {
        public string TourId { get; set; }
        public CreatorInfo? Creator { get; set; }
        public string TourDescription { get; set; }
        public ApprovalStatus? ApprovalStatus { get; set; }
        public int MaxGuests { get; set; }
        public string Location { get; set; }
        public int NumberOfDays { get; set; }
        public string TourName { get; set; }
        public double? Price { get; set; }
        public string TourImage { get; set; }

        public List<TourSchedule> Schedules { get; set; }
    }
}
