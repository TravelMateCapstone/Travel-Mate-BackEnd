using BusinessObjects.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class GroupParticipant
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupParticipantId { get; set; }

    public int? GroupId { get; set; }
    [JsonIgnore]
    public Group? Group { get; set; }

    public int? UserId { get; set; }
    [JsonIgnore]
    public ApplicationUser? User { get; set; }

    [Required]
    public bool JoinedStatus { get; set; }

    [Required]
    public DateTime RequestAt { get; set; }

    public DateTime JoinAt { get; set; }

}