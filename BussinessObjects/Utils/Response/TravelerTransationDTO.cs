using BusinessObjects.EnumClass;

namespace BusinessObjects.Utils.Response
{
    public class TravelerTransationDTO
    {
        public string? TourId { get; set; }
        public string TourName { get; set; }
        public string? ScheduleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ParticipantId { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? TransactionTime { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
