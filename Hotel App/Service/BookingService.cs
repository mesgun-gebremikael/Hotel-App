using System;
using System.Linq;
using Hotel_App.Data;
using Hotel_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_App.Services
{
    public class BookingService
    {
        private readonly AppDbContext _db;

        public BookingService(AppDbContext db)
        {
            _db = db;
        }

        public void ListBookings()
        {
            var bookings = _db.Bookings
                .Include(b => b.Room)
                .Include(b => b.Customer)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            Console.WriteLine("\n--- BOKNINGAR ---");
            if (bookings.Count == 0)
            {
                Console.WriteLine("Inga bokningar finns.");
                return;
            }

            foreach (var b in bookings)
            {
                Console.WriteLine($"{b.Id}. Rum {b.Room.RoomNumber} | {b.Customer.FirstName} {b.Customer.LastName} | {b.StartDate:yyyy-MM-dd} -> {b.EndDate:yyyy-MM-dd} | {b.Status}");
            }
        }

        public void CreateBooking()
        {
            Console.WriteLine("\n--- SKAPA BOKNING ---");

            // 1) välj kund
            var customers = _db.Customers.OrderBy(c => c.LastName).ToList();
            if (customers.Count == 0)
            {
                Console.WriteLine("Det finns inga kunder. Skapa kund först.");
                return;
            }

            Console.WriteLine("\nKunder:");
            foreach (var c in customers)
                Console.WriteLine($"{c.Id}. {c.FirstName} {c.LastName}");

            Console.Write("Välj kund ID: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Fel ID.");
                return;
            }

            var customer = _db.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
            {
                Console.WriteLine("Kunden hittades inte.");
                return;
            }

            // 2) välj rum
            var rooms = _db.Rooms.OrderBy(r => r.RoomNumber).ToList();
            if (rooms.Count == 0)
            {
                Console.WriteLine("Det finns inga rum. Skapa rum först.");
                return;
            }

            Console.WriteLine("\nRum:");
            foreach (var r in rooms)
            {
                Console.WriteLine($"{r.Id}. Rum {r.RoomNumber} | {r.Type} | Bas: {r.BaseCapacity} | Extra max: {r.ExtraBedsMax}");
            }

            Console.Write("Välj rum ID: ");
            if (!int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.WriteLine("Fel ID.");
                return;
            }

            var room = _db.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null)
            {
                Console.WriteLine("Rummet hittades inte.");
                return;
            }

            // 3) datum
            if (!TryReadDate("Startdatum (YYYY-MM-DD): ", out DateTime start))
                return;

            if (!TryReadDate("Slutdatum (YYYY-MM-DD): ", out DateTime end))
                return;

            start = start.Date;
            end = end.Date;

            var today = DateTime.Today;

            if (start < today)
            {
                Console.WriteLine("Du kan inte boka i dåtid.");
                return;
            }

            if (end <= start)
            {
                Console.WriteLine("Slutdatum måste vara efter startdatum.");
                return;
            }

            // 4) extrasängar om dubbelrum
            int extraBedsUsed = 0;
            if (room.Type == RoomType.Double && room.ExtraBedsMax > 0)
            {
                Console.Write($"Extrasängar (0-{room.ExtraBedsMax}): ");
                if (!int.TryParse(Console.ReadLine(), out extraBedsUsed) || extraBedsUsed < 0 || extraBedsUsed > room.ExtraBedsMax)
                {
                    Console.WriteLine("Fel antal extrasängar.");
                    return;
                }
            }

            // 5) kolla overlap (ingen dubbelbokning)
            bool conflict = _db.Bookings.Any(b =>
                b.RoomId == roomId &&
                b.Status == BookingStatus.Active &&
                start < b.EndDate &&
                end > b.StartDate
            );

            if (conflict)
            {
                Console.WriteLine("Rummet är redan bokat under den perioden.");
                return;
            }

            // 6) skapa bokning
            var booking = new Booking
            {
                CustomerId = customerId,
                RoomId = roomId,
                StartDate = start,
                EndDate = end,
                ExtraBedsUsed = extraBedsUsed,
                Status = BookingStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _db.Bookings.Add(booking);
            _db.SaveChanges();

            Console.WriteLine("Bokningen är skapad!");
        }

        private bool TryReadDate(string prompt, out DateTime date)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (!DateTime.TryParse(input, out date))
            {
                Console.WriteLine("Fel datumformat. Skriv t.ex. 2026-03-10");
                return false;
            }

            return true;
        }
    }
}