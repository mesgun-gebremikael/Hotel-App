using System;

namespace Hotel_App.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ExtraBedsUsed { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Active;

        public Invoice Invoice { get; set; } = null!;
    }
}