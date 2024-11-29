using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Entities
{
    public class Tour
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? TourId { get; set; } // Unique identifier for the tour

        [BsonElement("tourName")]
        public string TourName { get; set; } // Name of the tour

        [BsonElement("price")]
        public double? Price { get; set; } // Name of the tour

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; } // Tour start date

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; } // Tour end date

        [BsonElement("numberOfDays")]
        public int NumberOfDays { get; set; } // Ví dụ: 2
        [BsonElement("numberOfNights")]
        public int NumberOfNights { get; set; } // Ví dụ: 3

        [BsonElement("location")]
        public string Location { get; set; } // Location of the tour

        [BsonElement("maxGuests")]
        public int MaxGuests { get; set; } // Maximum number of guests allowed

        [BsonElement("tourStatus")]
        public bool? TourStatus { get; set; } = true;

        [BsonElement("registeredGuests")]
        public int RegisteredGuests { get; set; } = 0;

        [BsonElement("approvalStatus")]
        public bool? ApprovalStatus { get; set; } // Status: pending, approved, rejected

        [BsonElement("tourImage")]
        public string TourImage { get; set; }

        [BsonElement("creator")]
        public CreatorInfo Creator { get; set; } // Information about the tour creator

        [BsonElement("participants")]
        public List<Participants>? Participants { get; set; }

        [BsonElement("itinerary")]
        public List<Itinerary> Itinerary { get; set; }

        [BsonElement("costDetails")]
        public List<CostDetail> CostDetails { get; set; }

        [BsonElement("additionalInfo")]
        public string AdditionalInfo { get; set; }

        public List<TourReview>? Reviews { get; set; }

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class TourReview
    {
        [BsonElement("ReviewerId")]
        public string ReviewerId { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("rating")]
        public int Rating { get; set; }
        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }
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
        [BsonElement("travelerId")]
        public int? Id { get; set; } // Unique ID of the traveler

        [BsonElement("registeredAt")]
        public DateTime RegisteredAt { get; set; }

        [BsonElement("paymentStatus")]
        public bool PaymentStatus { get; set; } // register, cọc, done
    }

    public class Itinerary
    {
        [BsonElement("day")]
        public int Day { get; set; } // Day of the itinerary

        [BsonElement("date")]
        public DateTime Date { get; set; } // Date of the day

        [BsonElement("activities")]
        public List<TourActivity> Activities { get; set; } // List of activities for the day
    }

    public class TourActivity
    {
        [BsonElement("time")]
        public string Time { get; set; } // Time of the activity (e.g., 08:00 - 09:00)

        [BsonElement("description")]
        public string Description { get; set; } // Description of the activity

        [BsonElement("activityAddress")]
        public string ActivityAddress { get; set; } // Description of the activity

        [BsonElement("activityAmount")]
        public decimal ActivityAmount { get; set; } // Description of the activity

        [BsonElement("activityImage")]
        public string activityImage { get; set; } // Description of the activity

    }

    public class CostDetail
    {
        [BsonElement("title")]
        public string Title { get; set; } // Cost item title (e.g., Travel Insurance)

        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }
    }
}
