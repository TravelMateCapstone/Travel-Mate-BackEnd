using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class LocalExtraDetailForm
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? FormId { get; set; } // MongoDB will generate an ID if this is null

        [BsonElement("createById")]
        public int? CreateById { get; set; }

        [BsonElement("questions")]
        public List<Question> Questions { get; set; } = new List<Question>();

        [BsonElement("services")]
        public List<Service> Services { get; set; } = new List<Service>();

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("latestUpdateAt")]
        public DateTime? LatestUpdate { get; set; }
    }

    public class Question
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("options")]
        public List<string> Options { get; set; } = new List<string>();
    }

    public class Service
    {

        [BsonElement("serviceName")]
        public string ServiceName { get; set; }

        [BsonElement("amount")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }
    }
}
