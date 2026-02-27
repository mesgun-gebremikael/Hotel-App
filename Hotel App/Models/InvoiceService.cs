using System;
using System.Linq;
using Hotel_App.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel_App.Services
{
    public class InvoiceService
    {
        private readonly AppDbContext _db;

        public InvoiceService(AppDbContext db)
        {
            _db = db;
        }

        public void ListInvoices()
        {
            var invoices = _db.Invoices
                .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
                .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
                .OrderByDescending(i => i.IssuedAt)
                .ToList();

            Console.WriteLine("\n--- FAKTUROR ---");

            if (invoices.Count == 0)
            {
                Console.WriteLine("Inga fakturor finns.");
                return;
            }

            foreach (var i in invoices)
            {
                var paidText = i.PaidAt.HasValue ? $"BETALD ({i.PaidAt.Value:yyyy-MM-dd})" : "OBETALD";
                Console.WriteLine($"{i.Id}. {paidText} | {i.TotalAmount} kr | Rum {i.Booking.Room.RoomNumber} | {i.Booking.Customer.FirstName} {i.Booking.Customer.LastName} | Bokad: {i.Booking.CreatedAt:yyyy-MM-dd}");
            }
        }

        public void RegisterPayment()
        {
            Console.WriteLine("\n--- REGISTRERA BETALNING ---");
            ListInvoices();

            Console.Write("\nSkriv faktura-ID att betala: ");
            if (!int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                Console.WriteLine("Fel ID.");
                return;
            }

            var invoice = _db.Invoices
                .Include(i => i.Booking)
                .FirstOrDefault(i => i.Id == invoiceId);

            if (invoice == null)
            {
                Console.WriteLine("Fakturan hittades inte.");
                return;
            }

            if (invoice.PaidAt.HasValue)
            {
                Console.WriteLine("Fakturan är redan betald.");
                return;
            }

            invoice.PaidAt = DateTime.UtcNow;
            _db.SaveChanges();

            Console.WriteLine("Betalning registrerad!");
        }
    }
}
