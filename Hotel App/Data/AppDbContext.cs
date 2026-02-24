using Hotel_App.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //  rumsnummer (så man inte kan ha två rum med samma nummer)
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.RoomNumber)
            .IsUnique();

        // 1 bokning har 1 faktura (one-to-one)
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Invoice)
            .WithOne(i => i.Booking)
            .HasForeignKey<Invoice>(i => i.BookingId);

        // Seed Rooms (minst 4)
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, RoomNumber = "101", Type = RoomType.Single, BaseCapacity = 1, ExtraBedsMax = 0, PricePerNight = 800 },
            new Room { Id = 2, RoomNumber = "102", Type = RoomType.Single, BaseCapacity = 1, ExtraBedsMax = 0, PricePerNight = 850 },
            new Room { Id = 3, RoomNumber = "201", Type = RoomType.Double, BaseCapacity = 2, ExtraBedsMax = 1, PricePerNight = 1100 },
            new Room { Id = 4, RoomNumber = "202", Type = RoomType.Double, BaseCapacity = 2, ExtraBedsMax = 2, PricePerNight = 1250 }
        );

        // Seed Customers (minst 4)
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "Sara", LastName = "Ali", Email = "sara@example.com", Phone = "0700000001" },
            new Customer { Id = 2, FirstName = "Ahmed", LastName = "Yusuf", Email = "ahmed@example.com", Phone = "0700000002" },
            new Customer { Id = 3, FirstName = "Lina", LastName = "Hassan", Email = "lina@example.com", Phone = "0700000003" },
            new Customer { Id = 4, FirstName = "Jonas", LastName = "Svensson", Email = "jonas@example.com", Phone = "0700000004" }
        );
    }
}