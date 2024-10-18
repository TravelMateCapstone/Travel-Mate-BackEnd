namespace BussinessObjects.Entities
{
    public class UserHome
    {
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int MaxGuests { get; set; }
        public string? GuestPreferences { get; set; }
        public string? HouseRules { get; set; }
        public bool IsPrivateRoom { get; set; }
        public string? RoomMate { get; set; }
        public string? Amenities { get; set; }
        public string? Description { get; set; }
        public string? Transportation { get; set; }

        public ICollection<HomePhoto>? HomePhotos { get; set; }
    }
}
