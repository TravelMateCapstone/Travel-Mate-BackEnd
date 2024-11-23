using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class TravelerExtraDetailForm
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("createById")]
        public int? CreateById { get; set; }

        [BsonElement("travelerId")]
        public int? TravelerId { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("requestStatus")]
        public bool RequestStatus { get; set; }

        [BsonElement("sendAt")]
        public DateTime? SendAt { get; set; }

        [BsonElement("latestUpdateAt")]
        public DateTime? LatestUpdateAt { get; set; }

        [BsonElement("questions")]
        public List<Question>? Questions { get; set; } = new List<Question>();

        [BsonElement("services")]
        public List<Service>? Services { get; set; } = new List<Service>();

        [BsonElement("answeredQuestions")]
        public List<AnsweredQuestion> AnsweredQuestions { get; set; } = new List<AnsweredQuestion>();

        [BsonElement("answeredServices")]
        public List<AnsweredService> AnsweredServices { get; set; } = new List<AnsweredService>();
    }

    public class AnsweredQuestion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? QuestionId { get; set; }

        [BsonElement("answer")]
        public List<string>? Answer { get; set; }
    }

    public class AnsweredService
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ServiceId { get; set; }

        public decimal Total { get; set; }
    }

}
