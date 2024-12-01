using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using MongoDB.Bson.Serialization.Attributes;

namespace BusinessObjects.Utils.Request
{
    public class TourDto
    {
        [BsonElement("tourId")]
        public string? TourId { get; set; }

        [BsonElement("creator")]
        public CreatorInfo? Creator { get; set; }

        [BsonElement("tourName")]
        public string TourName { get; set; } // Name of the tour

        [BsonElement("price")]
        public double? Price { get; set; } // Price of the tour

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; } // Tour start date

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; } // Tour end date

        [BsonElement("numberOfDays")]
        public int NumberOfDays { get; set; } // Duration of the tour in days

        [BsonElement("numberOfNights")]
        public int NumberOfNights { get; set; }

        [BsonElement("approvalStatus")]
        public ApprovalStatus? ApprovalStatus { get; set; }

        [BsonElement("location")]
        public string Location { get; set; } // Location of the tour

        [BsonElement("maxGuests")]
        public int MaxGuests { get; set; } // Maximum number of guests allowed

        [BsonElement("tourImage")]
        public string TourImage { get; set; } // Tour image URL

        [BsonElement("itinerary")]
        public List<Itinerary> Itinerary { get; set; } // Tour itinerary

        [BsonElement("costDetails")]
        public List<CostDetail> CostDetails { get; set; } // Cost details for the tour

        [BsonElement("additionalInfo")]
        public string AdditionalInfo { get; set; } // Additional information about the tour
    }
}
