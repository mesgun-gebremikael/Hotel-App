using System;

namespace Hotel_App.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public decimal TotalAmount { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}
