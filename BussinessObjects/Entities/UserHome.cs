using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities
{
    public class UserHome
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserHomeId { get; set; }

        public int UserId { get; set; }
        // Maximum number of guests
        public int MaxGuests { get; set; }

        // Preferences of the guests
        public string GuestPreferences { get; set; }

        // House rules
        //public string HouseRules { get; set; }

        public string AllowedSmoking { get; set; }

        public string RoomDescription { get; set; }

        public string RoomType { get; set; }

        // Indicate if the room is private
        //public bool IsPrivateRoom { get; set; }

        // Information about roommates
        public string RoomMateInfo { get; set; }

        // Amenities available
        public string Amenities { get; set; }

        // Description of the home
        //public string Description { get; set; }

        // Transportation information
        public string Transportation { get; set; }

        public string OverallDescription { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
        // Liên kết với HomePhoto
        public virtual ICollection<HomePhoto>? HomePhotos { get; set; }

    }
}
