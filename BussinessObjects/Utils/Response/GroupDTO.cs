namespace BusinessObjects.Utils.Response
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public string Location { get; set; }

        public DateTime CreateAt { get; set; }

        public string? Description { get; set; }
        public string? GroupImageUrl { get; set; }

        public int CreatedById { get; set; }

        public ICollection<GroupParticipantDTO>? Participants { get; set; }
    }
}
