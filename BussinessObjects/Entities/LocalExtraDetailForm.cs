using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class LocalExtraDetailForm
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // MongoDB will generate an ID if this is null

        [BsonElement("createById")]
        public int? CreateById { get; set; }

        [BsonElement("questions")]
        public List<Question> Questions { get; set; } = new List<Question>();

        [BsonElement("services")]
        public List<Service> Services { get; set; } = new List<Service>();

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("latestUpdateAt")]
        public DateTime? LatestUpdateAt { get; set; }
    }

    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("isRequired")]
        public bool IsRequired { get; set; } = false;

        [BsonElement("options")]
        public List<string> Options { get; set; } = new List<string>();

        [BsonElement("latestModified")]
        public DateTime? LatestModified { get; set; }
    }

    public class Service
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("serviceName")]
        public string ServiceName { get; set; }

        [BsonElement("amount")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }
    }
}
