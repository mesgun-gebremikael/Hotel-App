using System;
using System.Linq;
using Hotel_App.Data;
using Hotel_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_App.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _db;

        public CustomerService(AppDbContext db)
        {
            _db = db;
        }

        public void ListCustomers()
        {
            var customers = _db.Customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();

            Console.WriteLine("\n--- KUNDER ---");
            if (customers.Count == 0)
            {
                Console.WriteLine("Inga kunder finns.");
                return;
            }

            foreach (var c in customers)
            {
                Console.WriteLine($"{c.Id}. {c.FirstName} {c.LastName} | {c.Email} | {c.Phone}");
            }
        }

        public void AddCustomer()
        {
            Console.WriteLine("\n--- LÄGG TILL KUND ---");

            Console.Write("Förnamn: ");
            var firstName = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Efternamn: ");
            var lastName = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Telefon: ");
            var phone = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Förnamn och efternamn måste fyllas i.");
                return;
            }

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone
            };

            _db.Customers.Add(customer);
            _db.SaveChanges();

            Console.WriteLine("Kunden är sparad!");
        }

        public void UpdateCustomer()
        {
            Console.WriteLine("\n--- UPPDATERA KUND ---");
            ListCustomers();

            Console.Write("\nSkriv ID på kund att uppdatera: ");
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

            Console.Write($"Förnamn (nu: {customer.FirstName}) eller Enter: ");
            var firstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName))
                customer.FirstName = firstName.Trim();

            Console.Write($"Efternamn (nu: {customer.LastName}) eller Enter: ");
            var lastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName))
                customer.LastName = lastName.Trim();

            Console.Write($"Email (nu: {customer.Email}) eller Enter: ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                customer.Email = email.Trim();

            Console.Write($"Telefon (nu: {customer.Phone}) eller Enter: ");
            var phone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone))
                customer.Phone = phone.Trim();

            _db.SaveChanges();
            Console.WriteLine("Kunden är uppdaterad!");
        }

        public void DeleteCustomer()
        {
            Console.WriteLine("\n--- RADERA KUND ---");
            ListCustomers();

            Console.Write("\nSkriv ID på kund att radera: ");
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

            
            _db.Customers.Remove(customer);
            _db.SaveChanges();

            Console.WriteLine("Kunden är raderad!");
        }
    }
}