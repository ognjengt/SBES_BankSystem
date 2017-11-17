using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            BankServer server = new BankServer();
            server.Start();

            // Iscitavanje iz fajla, upisivanje u BazuKorisnika
            

            BankDB.BazaKorisnika.Add("marko", new User("marko", "1111", "admin"));
            Console.ReadKey();
        }
    }
}
