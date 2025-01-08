using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class PastTripPost
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string ScheduleId { get; set; }
        public string TourId { get; set; }
        public int TravelerId { get; set; }
        public string? TravelerName { get; set; }
        public string? TravelerAvatar { get; set; }
        public bool? IsCaptionEdit { get; set; }
        public string? Caption { get; set; }
        public int? Star { get; set; }
        public string? Location { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<string>? TripImages { get; set; }

        //LOCATION
        public int? LocalId { get; set; }
        public string? LocalName { get; set; }
        public string? LocalAvatar { get; set; }
        public string? Comment { get; set; }
        public bool? IsCommentEdited { get; set; }

    }
}
