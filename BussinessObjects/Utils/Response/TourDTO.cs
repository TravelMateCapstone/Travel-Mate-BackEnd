namespace BusinessObjects.Utils.Response
{
    public class TourDTO
    {
        public string TourId { get; set; }
        public int LocalId { get; set; }
        public int RegisteredGuests { get; set; }
        public string TourDescription { get; set; }
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
