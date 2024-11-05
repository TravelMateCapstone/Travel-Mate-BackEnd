﻿namespace BusinessObjects.Utils.Response
{
    public class PostCommentDTO
    {
        public int CommentId { get; set; }
        public string Commentor { get; set; }
        public bool IsEdited { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; }
    }
}