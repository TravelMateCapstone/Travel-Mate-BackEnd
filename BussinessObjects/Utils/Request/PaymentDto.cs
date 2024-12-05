namespace BusinessObjects.Utils.Request
{
    public class PaymentDto
    {
        //ten tour
        public string TourName { get; set; }

        public string tourId { get; set; }
        public int localId { get; set; }

        public int travelerId { get; set; }
        //so tien
        public int Amount { get; set; }
    }
}
