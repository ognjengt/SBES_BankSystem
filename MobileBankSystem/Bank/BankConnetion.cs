using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankConnetion : IBankConnection
    {
        public void AddAccount()
        {
            Console.WriteLine("AddAccount called");
        }

        public bool CheckLogin(string username, string password)
        {
            if (BankDB.BazaKorisnika.ContainsKey(username))
            {
                if (BankDB.BazaKorisnika[username].Password == password)
                {
                    return true;
                }
            }
            return false;



        }

        public void Transfer()
        {
            Console.WriteLine("Trasfer called");
        }
        
    }
}
