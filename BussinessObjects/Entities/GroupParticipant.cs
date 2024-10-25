using BusinessObjects.Entities;
using System.ComponentModel.DataAnnotations;

public class GroupParticipant
{
    [Required]
    public int GroupId { get; set; }
    //[JsonIgnore]
    public Group? Group { get; set; }

    [Required]
    public int UserId { get; set; }

    //[JsonIgnore]
    public ApplicationUser? User { get; set; }
    [Required]
    public bool JoinedStatus { get; set; }

}