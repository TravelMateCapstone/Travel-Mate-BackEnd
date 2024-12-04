namespace TravelMateAPI.Services.Contract
{
    public class CreateContractRequest
    {
        public int TravelerId { get; set; } // ID của người đi du lịch
        public int LocalId { get; set; }    // ID của người địa phương
        public string TourId { get; set; }     // ID của Tour
        public string Details { get; set; } // Chi tiết hợp đồng
        public string TravelerSignature { get; set; } // Chữ ký số của người đi du lịch
        public string LocalSignature { get; set; }    // Chữ ký số của người địa phương
    }

}
