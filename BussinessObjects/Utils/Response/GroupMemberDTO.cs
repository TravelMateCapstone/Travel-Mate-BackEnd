namespace BusinessObjects.Utils.Response
{
    public class GroupMemberDTO
    {
        public int Id { get; set; }
        public string MemberName { get; set; }
        public string Address { get; set; }

        public DateTime RequestAt { get; set; }
        public DateTime JoinAt { get; set; }
    }
}
