namespace BusinessObjects.Utils.Response
{
    public class PastTripPostDTO
    {
        public int PastTripPostId { get; set; }
        public string ScheduleId { get; set; }
        public string Location { get; set; }

        public int TravelerId { get; set; }

        public string TravelerName { get; set; }

        public string TravelerAvatar { get; set; }

        public bool IsPublic { get; set; }

        public string? Caption { get; set; }

        public int LocalId { get; set; }

        public string LocalName { get; set; }

        public string LocalAvatar { get; set; }

        public string Review { get; set; }

        public int Star { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
