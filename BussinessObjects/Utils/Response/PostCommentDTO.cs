namespace BusinessObjects.Utils.Response
{
    public class PostCommentDTO
    {
        public int PostCommentId { get; set; }
        public int CommentedById { get; set; }
        public string Commentor { get; set; }
        public string CommentorAvatar { get; set; }
        public bool IsEdited { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
