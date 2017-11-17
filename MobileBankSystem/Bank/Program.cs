using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            BankServer server = new BankServer();
            server.Start();

            BankDB.BazaKorisnika.Add("marko", new User("marko", "1111", "admin"));
            Console.ReadKey();
        }
    }
}
