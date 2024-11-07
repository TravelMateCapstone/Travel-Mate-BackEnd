namespace BusinessObjects.Utils.Response
{
    public class GroupPostDTO
    {
        public int PostId { get; set; }
        public int PostById { get; set; }
        public string PostCreatorName { get; set; }
        public string PostCreatorAvatar { get; set; }
        public string Title { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<string> PostPhotos { get; set; }
        //public List<PostCommentDTO> Comments { get; set; }
    }

}
