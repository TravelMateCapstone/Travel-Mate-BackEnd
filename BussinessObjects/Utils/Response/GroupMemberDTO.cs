namespace BusinessObjects.Utils.Response
{
    public class GroupMemberDTO
    {
        public int UserId { get; set; }
        public string MemberName { get; set; }
        public string MemberAvatar { get; set; }
        public string City { get; set; }

        public bool JoinedStatus { get; set; }

        public DateTime RequestAt { get; set; }
        public DateTime JoinAt { get; set; }
    }
}
