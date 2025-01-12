namespace TravelMateAPI.Services.Contract
{
    public class TravelerContractDTO
    {
        public int LocalId { get; set; }
        public string TourId { get; set; }
        public string ScheduleId { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public ProfileDTO LocalProfile { get; set; }
        public AccountDTO Account { get; set; }
    }
}
