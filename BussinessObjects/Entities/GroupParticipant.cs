using BusinessObjects.Entities;
using System.ComponentModel.DataAnnotations;

public class GroupParticipant
{
    public int? GroupId { get; set; }
    public Group? Group { get; set; }

    public int? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    [Required]
    public bool JoinedStatus { get; set; }

    [Required]
    public DateTime RequestAt { get; set; }

    public DateTime JoinAt { get; set; }

}