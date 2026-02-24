using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hotel_App.Models.Enums;

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
