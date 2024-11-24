using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int? MessageId { get; set; }

        [BsonElement("senderId")]
        public int SenderId { get; set; }
        [BsonElement("receiverId")]
        public int ReceiverId { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("sentAt")]
        public DateTime? SentAt { get; set; }
    }
}
