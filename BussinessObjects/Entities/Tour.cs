using BusinessObjects.EnumClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class Tour
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? TourId { get; set; }

        [BsonElement("tourName")]
        public string TourName { get; set; }

        [BsonElement("price")]
        public double? Price { get; set; }

        [BsonElement("numberOfDays")]
        public int NumberOfDays { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("maxGuests")]
        public int MaxGuests { get; set; }

        [BsonElement("tourStatus")]
        public bool? TourStatus { get; set; } = true;

        [BsonElement("approvalStatus")]
        public ApprovalStatus? ApprovalStatus { get; set; }

        [BsonElement("tourImage")]
        public string TourImage { get; set; }

        [BsonElement("creator")]
        public CreatorInfo Creator { get; set; }

        [BsonElement("itinerary")]
        public List<Itinerary> Itinerary { get; set; }

        [BsonElement("costDetails")]
        public List<CostDetail> CostDetails { get; set; }

        [BsonElement("additionalInfo")]
        public string AdditionalInfo { get; set; }

        [BsonElement("tourDescription")]
        public string TourDescription { get; set; }

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("schedules")]
        public List<TourSchedule> Schedules { get; set; } = new();
    }

    public class TourSchedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ScheduleId { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("participants")]
        public List<Participants>? Participants { get; set; }

        [BsonElement("activeStatus")]
        public bool? ActiveStatus { get; set; }
    }

    public class CreatorInfo
    {
        [BsonElement("creatorId")]
        public int Id { get; set; }

        [BsonElement("fullname")]
        public string? Fullname { get; set; }

        [BsonElement("avatar")]
        public string? AvatarUrl { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("rating")]
        public double? Rating { get; set; }

        [BsonElement("totalTrips")]
        public int? TotalTrips { get; set; }

        [BsonElement("joinedAt")]
        public DateTime? JoinedAt { get; set; }
    }

    public class Participants
    {
        [BsonElement("participantId")]
        public int ParticipantId { get; set; }

        [BsonElement("fullName")]
        public string? FullName { get; set; }

        [BsonElement("gender")]
        public string? Gender { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("registeredAt")]
        public DateTime RegisteredAt { get; set; }

        [BsonElement("orderCode")]
        public long? OrderCode { get; set; }

        [BsonElement("paymentStatus")]
        public PaymentStatus? PaymentStatus { get; set; }

        [BsonElement("transactionTime")]
        public DateTime? TransactionTime { get; set; }

        [BsonElement("totalAmount")]
        public decimal? TotalAmount { get; set; }

        [BsonElement("postId")]
        public string? PostId { get; set; }
    }

    public class Itinerary
    {
        [BsonElement("day")]
        public int Day { get; set; }

        [BsonElement("activities")]
        public List<TourActivity> Activities { get; set; }
    }

    public class TourActivity
    {
        [BsonElement("startTime")]
        public TimeOnly StartTime { get; set; }

        [BsonElement("endTime")]
        public TimeOnly EndTime { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("activityAddress")]
        public string? ActivityAddress { get; set; }

        [BsonElement("activityAmount")]
        public decimal? ActivityAmount { get; set; }

        [BsonElement("activityImage")]
        public string? activityImage { get; set; }
    }

    public class CostDetail
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }
    }
}