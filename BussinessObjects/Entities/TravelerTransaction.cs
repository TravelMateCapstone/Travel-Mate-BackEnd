using BusinessObjects.EnumClass;

namespace BusinessObjects.Entities
{
    public class TravelerTransaction
    {
        //important
        public string Id { get; set; }
        public string TourId { get; set; }
        public string? TourName { get; set; }
        public string ScheduleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ParticipantId { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? TransactionTime { get; set; }
        public double? TotalAmount { get; set; }
    }
}
