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
            // Napraviti dictionary koji ce da cuva korisnike, kljuc racun
            BankDB.Baza.Add("000111",new User("asd", "asd", "asd", "000111"));
        }
    }
}
