namespace TravelMateAPI.Services.RecommenTourService
{
    //public class TravelerActivityTourDTO
    //{
    //    public int TravelerId { get; set; }
    //    public List<string> TourIds { get; set; } = new List<string>();
    //    public List<int> ActivityIds { get; set; } = new List<int>();
    //}
    public class TravelerActivityTourDTO
    {
        public int TravelerId { get; set; }
        public List<TourPurchaseDTO> Tours { get; set; } = new List<TourPurchaseDTO>();
        public List<int> ActivityIds { get; set; } = new List<int>();
    }

    public class TourPurchaseDTO
    {
        public string TourId { get; set; }
        public int PurchaseCount { get; set; }
    }
}
