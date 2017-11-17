using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankConnetion : IBankConnection
    {
      

        public void AddAccount(User u)
        {
            // using (ResXResourceWriter rw)
            //ovaj koristiti: "" ResourceWriter rw=


            if (BankDB.BazaRacuna.ContainsKey(u.Username)) {

                Console.WriteLine("Ovaj korisnik vec postoji");
                return;

            }
            BankDB.BazaKorisnika.Add(u.Username,u);
            foreach (var korisnik in BankDB.BazaKorisnika)
            {
                Console.WriteLine(korisnik.Key);
            }
        }

        public User CheckLogin(string username, string password)
        {
            if (BankDB.BazaKorisnika.ContainsKey(username))
            {
                if (BankDB.BazaKorisnika[username].Password == password)
                {
                    User u = BankDB.BazaKorisnika[username];
                    return u;
                }
            }

            return null;



        }

        public void Transfer()
        {
            Console.WriteLine("Trasfer called");
        }
        
    }
}
