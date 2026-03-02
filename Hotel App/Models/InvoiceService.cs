using System;
using System.Linq;
using Hotel_App.Data;
using Hotel_App.Models;
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
                var invoiceStatus = i.PaidAt.HasValue ? "BETALD" : "OBETALD";
                var paidText = i.PaidAt.HasValue ? $"{i.PaidAt.Value:yyyy-MM-dd}" : "-";

                Console.WriteLine(
                    $"{i.Id}. {invoiceStatus} | {i.TotalAmount} kr | " +
                    $"Rum {i.Booking.Room.RoomNumber} | " +
                    $"{i.Booking.Customer.FirstName} {i.Booking.Customer.LastName} | " +
                    $"Bokning: {i.Booking.Status} | " +
                    $"Skapad: {i.Booking.CreatedAt:yyyy-MM-dd} | " +
                    $"Betald: {paidText}"
                );
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

            // Blockera betalning om bokningen är annullerad
            if (invoice.Booking.Status == BookingStatus.Cancelled)
            {
                Console.WriteLine("Kan inte registrera betalning. Bokningen är annullerad.");
                return;
            }

            invoice.PaidAt = DateTime.UtcNow;

            //  När betald faktura -> bokning blir Paid
            invoice.Booking.Status = BookingStatus.Paid;

            _db.SaveChanges();

            Console.WriteLine("Betalning registrerad!");
        }
    }
}
