using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Hotel_App.Services;

namespace Hotel_App
{
    public class Menu
    {
        public void InvoicesMenu(InvoiceService invoiceService)
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- FAKTURA MENY ---");
                Console.WriteLine("1. Lista fakturor");
                Console.WriteLine("2. Registrera betalning");
                Console.WriteLine("0. Tillbaka");
                Console.Write("Välj: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        invoiceService.ListInvoices();
                        break;
                    case "2":
                        invoiceService.RegisterPayment();
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
    }
}