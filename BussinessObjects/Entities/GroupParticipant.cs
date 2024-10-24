using BussinessObjects.Entities;

public class GroupParticipant
{

    public int GroupId { get; set; }
    public Group? Group { get; set; }

    public int UserId { get; set; }

    public ApplicationUser? User { get; set; }
}