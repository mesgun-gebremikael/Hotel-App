using System.Collections.Generic;

namespace Hotel_App.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

        public List<Booking> Bookings { get; set; } = new();
    }
}