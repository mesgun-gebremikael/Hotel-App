using System.Collections.Generic;

namespace Hotel_App.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = "";
        public RoomType Type { get; set; }
        public int BaseCapacity { get; set; }
        public int ExtraBedsMax { get; set; }
        public decimal PricePerNight { get; set; }

        public List<Booking> Bookings { get; set; } = new();
    }
}