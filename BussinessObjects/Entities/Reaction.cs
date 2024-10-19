namespace BussinessObjects.Entities
{
    public class Reaction
    {
        public int ReactedById { get; set; }
        public ApplicationUser? ReactedByUser { get; set; }

        public int PostId { get; set; }
        public GroupPost? GroupPost { get; set; }

    }
}
