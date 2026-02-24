using Hotel_App.Data;
using Hotel_App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("HotelDb");

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var db = new AppDbContext(options);
var roomService = new RoomService(db);

bool running = true;

while (running)
{
    Console.WriteLine("\n==== HOTEL APP ====");
    Console.WriteLine("1. Rum");
    Console.WriteLine("0. Avsluta");
    Console.Write("Välj: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            RoomsMenu(roomService);
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Fel val.");
            break;
    }
}

static void RoomsMenu(RoomService roomService)
{
    bool back = false;

    while (!back)
    {
        Console.WriteLine("\n--- RUM MENY ---");
        Console.WriteLine("1. Lista rum");
        Console.WriteLine("2. Lägg till rum");
        Console.WriteLine("3. Uppdatera rum");
        Console.WriteLine("4. Radera rum");
        Console.WriteLine("0. Tillbaka");
        Console.Write("Välj: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                roomService.ListRooms();
                break;
            case "2":
                roomService.AddRoom();
                break;
            case "3":
                roomService.UpdateRoom();
                break;
            case "4":
                roomService.DeleteRoom();
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Fel val.");
                break;
        }
    }
}