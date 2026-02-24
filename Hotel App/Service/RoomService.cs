using System;
using System.Linq;
using Hotel_App.Data;
using Hotel_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_App.Services
{
    public class RoomService
    {
        private readonly AppDbContext _db;

        public RoomService(AppDbContext db)
        {
            _db = db;
        }

        public void ListRooms()
        {
            var rooms = _db.Rooms
                .OrderBy(r => r.RoomNumber)
                .ToList();

            Console.WriteLine("\n--- RUM ---");
            if (rooms.Count == 0)
            {
                Console.WriteLine("Inga rum finns.");
                return;
            }

            foreach (var r in rooms)
            {
                Console.WriteLine($"{r.Id}. Rum {r.RoomNumber} | {r.Type} | Bas: {r.BaseCapacity} | Extrasäng max: {r.ExtraBedsMax} | Pris: {r.PricePerNight} kr");
            }
        }

        public void AddRoom()
        {
            Console.WriteLine("\n--- LÄGG TILL RUM ---");

            Console.Write("Rumsnummer: ");
            var roomNumber = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(roomNumber))
            {
                Console.WriteLine("Rumsnummer får inte vara tomt.");
                return;
            }

            var exists = _db.Rooms.Any(r => r.RoomNumber == roomNumber);
            if (exists)
            {
                Console.WriteLine("Det finns redan ett rum med det rumsnumret.");
                return;
            }

            Console.Write("Typ (1 = Single, 2 = Double): ");
            var typeInput = Console.ReadLine();

            if (!int.TryParse(typeInput, out int typeVal) || (typeVal != 1 && typeVal != 2))
            {
                Console.WriteLine("Fel typ. Välj 1 eller 2.");
                return;
            }

            var roomType = (RoomType)typeVal;

            int baseCapacity = roomType == RoomType.Single ? 1 : 2;

            int extraBedsMax = 0;
            if (roomType == RoomType.Double)
            {
                Console.Write("Extrasäng max (0-2): ");
                if (!int.TryParse(Console.ReadLine(), out extraBedsMax) || extraBedsMax < 0 || extraBedsMax > 2)
                {
                    Console.WriteLine("Fel värde. Skriv 0, 1 eller 2.");
                    return;
                }
            }

            Console.Write("Pris per natt (t.ex. 850): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Fel pris.");
                return;
            }

            var room = new Room
            {
                RoomNumber = roomNumber,
                Type = roomType,
                BaseCapacity = baseCapacity,
                ExtraBedsMax = extraBedsMax,
                PricePerNight = price
            };

            _db.Rooms.Add(room);
            _db.SaveChanges();

            Console.WriteLine("Rummet är sparat!");
        }

        public void UpdateRoom()
        {
            Console.WriteLine("\n--- UPPDATERA RUM ---");
            ListRooms();

            Console.Write("\nSkriv ID på rum att uppdatera: ");
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

            Console.Write($"Nytt pris per natt (nu: {room.PricePerNight}) eller Enter för att hoppa: ");
            var priceInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(priceInput))
            {
                if (!decimal.TryParse(priceInput, out decimal newPrice) || newPrice < 0)
                {
                    Console.WriteLine("Fel pris.");
                    return;
                }
                room.PricePerNight = newPrice;
            }

            if (room.Type == RoomType.Double)
            {
                Console.Write($"Ny extrasäng max (nu: {room.ExtraBedsMax}) eller Enter för att hoppa: ");
                var extraInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(extraInput))
                {
                    if (!int.TryParse(extraInput, out int newExtra) || newExtra < 0 || newExtra > 2)
                    {
                        Console.WriteLine("Fel värde (0-2).");
                        return;
                    }
                    room.ExtraBedsMax = newExtra;
                }
            }

            _db.SaveChanges();
            Console.WriteLine("Rummet är uppdaterat!");
        }

        public void DeleteRoom()
        {
            Console.WriteLine("\n--- RADERA RUM ---");
            ListRooms();

            Console.Write("\nSkriv ID på rum att radera: ");
            if (!int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.WriteLine("Fel ID.");
                return;
            }

            var room = _db.Rooms
                .Include(r => r.Bookings)
                .FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                Console.WriteLine("Rummet hittades inte.");
                return;
            }

            // Enkel kontroll
            if (room.Bookings.Any(b => b.Status == BookingStatus.Active))
            {
                Console.WriteLine("Kan inte radera rummet. Det finns aktiva bokningar.");
                return;
            }

            _db.Rooms.Remove(room);
            _db.SaveChanges();

            Console.WriteLine("Rummet är raderat!");
        }
    }
}