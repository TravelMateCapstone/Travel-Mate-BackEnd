using BussinessObjects.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }
    public string GroupName { get; set; }

    public string Location { get; set; }

    public string? Description { get; set; }
    public string? GroupImageUrl { get; set; }

    public int CreatedById { get; set; }

    public ApplicationUser? CreatedByUser { get; set; }
    public ICollection<GroupParticipant>? GroupParticipants { get; set; }
    public ICollection<GroupPost>? GroupPosts { get; set; }



}
