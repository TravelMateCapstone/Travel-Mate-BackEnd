using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Entities
{
    public class UserHome
    {
        [Key]

        public int UserId { get; set; }
        // Maximum number of guests
        public int MaxGuests { get; set; }

        // Preferences of the guests
        public string GuestPreferences { get; set; }

        // House rules
        public string HouseRules { get; set; }

        // Indicate if the room is private
        public bool IsPrivateRoom { get; set; }

        // Information about roommates
        public string RoomMateInfo { get; set; }

        // Amenities available
        public string Amenities { get; set; }

        // Description of the home
        public string Description { get; set; }

        // Transportation information
        public string Transportation { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }

    }
}
