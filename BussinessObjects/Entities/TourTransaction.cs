using BusinessObjects.EnumClass;

namespace BusinessObjects.Entities
{
    public class TourTransaction
    {
        public string Id { get; set; }
        public string TourId { get; set; }
        public string? TourName { get; set; }
        public string ScheduleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ParticipantId { get; set; }
        public PaymentStatus? TransactionStatus { get; set; }
        public DateTime? TransactionTime { get; set; }
        public int LocalId { get; set; }
        public string? LocalName { get; set; }
        public int? TravelerId { get; set; }
        public string? TravelerName { get; set; }
        public double? Amount { get; set; }
        public double? LastAmount { get; set; }
    }
}
