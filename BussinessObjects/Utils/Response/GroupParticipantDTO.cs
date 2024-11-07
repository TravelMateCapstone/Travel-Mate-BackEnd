namespace BusinessObjects.Utils.Response
{
    public class GroupParticipantDTO
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public bool JoinedStatus { get; set; }

        public DateTime RequestAt { get; set; }

        public DateTime JoinAt { get; set; }
    }
}
