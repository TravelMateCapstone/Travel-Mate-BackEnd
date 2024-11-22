namespace BusinessObjects.Utils.Request
{
    public class PastTripPostInputDTO
    {
        public string Location { get; set; }
        public bool IsPublic { get; set; }
        public string Caption { get; set; }
        public string Review { get; set; }
        public int Star { get; set; }
        public List<PostPhotoInputDTO> PostPhotos { get; set; }
    }
}
