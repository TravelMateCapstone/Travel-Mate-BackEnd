using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("senderId")]
        public int SenderId { get; set; }
        [BsonElement("receiverId")]
        public int ReceiverId { get; set; }

        [BsonElement("message")]
        public string Content { get; set; }

        [BsonElement("sentAt")]
        public DateTime? SentAt { get; set; }
    }
}
