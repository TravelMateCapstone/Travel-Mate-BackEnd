using TravelMateAPI.Services.FilterLocal;

namespace TravelMateAPI.Services.FilterTour
{
    public class TourWithUserDetailsDTO
    {
        // Thông tin về tour
        public string TourId { get; set; }
        public int LocalId { get; set; } // ID của Local (người tổ chức tour)
        //public int RegisteredGuests { get; set; } // Số khách đã đăng ký
        public int MaxGuests { get; set; } // Số lượng khách tối đa
        public string Location { get; set; } // Địa điểm tổ chức tour
        public List<DateTime> StartDates { get; set; } // Danh sách ngày khởi hành
        //public DateTime StartDate { get; set; } // Ngày bắt đầu
        public string TourDescription { get; set; }
        //public DateTime EndDate { get; set; } // Ngày kết thúc
        public int NumberOfDays { get; set; } // Số ngày của tour
        //public int NumberOfNights { get; set; } // Số đêm của tour
        public string TourName { get; set; } // Tên tour
        public double? Price { get; set; } // Giá tour
        public string TourImage { get; set; } // Hình ảnh đại diện của tour

        // Thông tin về người dùng (Local)
        public UserWithDetailNoToursDTO User { get; set; } // Chi tiết của Local tổ chức tour
    }
}
