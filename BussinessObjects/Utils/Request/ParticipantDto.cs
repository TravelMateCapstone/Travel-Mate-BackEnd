﻿namespace BusinessObjects.Utils.Request
{
    public class ParticipantDto
    {
        public int? ParticipantId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime RegisteredAt { get; set; }
        public bool PaymentStatus { get; set; } = false;
        public double Discount { get; set; } = 0;
        public decimal TotalAmount { get; set; }
    }
}
