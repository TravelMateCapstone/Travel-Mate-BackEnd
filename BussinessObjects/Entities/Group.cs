using BusinessObjects.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }

    //model validation
    //use asp-validation-summary (Tag Helper) in JS to inform the error message
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]
    [StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]
    public string GroupName { get; set; }

    [Required]
    [StringLength(20)]
    public string Location { get; set; }

    public DateTime CreateAt { get; set; }

    public string? Description { get; set; }
    [Url]
    public string? GroupImageUrl { get; set; }

    public int NumberOfParticipants { get; set; }

    //[Required]
    public int CreatedById { get; set; }
    [JsonIgnore]
    public ApplicationUser? CreatedByUser { get; set; }
    [JsonIgnore]
    public ICollection<GroupParticipant>? GroupParticipants { get; set; }
    public ICollection<GroupPost>? GroupPosts { get; set; }
}
