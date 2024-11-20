using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class TravelerExtraDetailForm
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? TravelerFormId { get; set; } // MongoDB will generate an ID if this is null

        [BsonElement("createById")]
        public int? CreateById { get; set; }

        [BsonElement("travelerId")]
        public int? TravelerId { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("questions")]
        public List<AnsweredQuestion> Questions { get; set; } = new List<AnsweredQuestion>();

        [BsonElement("services")]
        public List<AnsweredService> Services { get; set; } = new List<AnsweredService>();

        [BsonElement("requestStatus")]
        public bool RequestStatus { get; set; }

        [BsonElement("sendAt")]
        public DateTime? SendAt { get; set; }

        [BsonElement("latestUpdateAt")]
        public DateTime? LatestUpdateAt { get; set; }

    }

    public class AnsweredQuestion
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("options")]
        public List<string> Options { get; set; } = new List<string>();

        [BsonElement("answer")]
        public List<string>? Answer { get; set; }
    }

    public class AnsweredService
    {
        [BsonElement("serviceName")]
        public string ServiceName { get; set; }

        [BsonElement("amount")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }

        [BsonElement("total")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Total { get; set; }
    }

}
