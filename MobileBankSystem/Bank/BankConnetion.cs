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

        public bool Transfer(string myUsername, string myUsernameOnOperator, string operatorUsername, int value)
        {
            bool retVal = true;
            if (BankDB.BazaKorisnika.ContainsKey(myUsername))
            {
                Console.WriteLine("Predstavili ste se kao nepostojeci korisnik");
                retVal = false;
            }

            if (BankDB.BazaKorisnika.ContainsKey(operatorUsername))
            {
                Console.WriteLine("Nepostojeci operator");
                retVal = false;
            }

            if (value <= 0)
            {
                Console.WriteLine("Novcana suma mora biti pozitivan broj");
                retVal = false;
            }

            if (BankDB.BazaRacuna[myUsername].StanjeRacuna < value)
            {
                Console.WriteLine("Nemate dovoljno novcanih sredstava na racunu");
                retVal = false;
            }

            BankDB.BazaRacuna[myUsername].StanjeRacuna -= value;
            BankDB.BazaRacuna[operatorUsername].StanjeRacuna += value;

            /*  
                Treba pozvati operatera preko wcf-a i obavestiti ga da je korisnik izvrsio uplatu na njegov racun.
                Da bi se to uradilo potrebno je da znamo ip adresu korisnika i njegov port na kome slusa. Te podatke o 
                korisniku treba dodati prikom njegovog dodavanja u banku. 

                Dodavanje u banku treba razdvojiti na dva slucaja:
                    * dodavanje korisnika -> dodavanje korisnika je ovo sto trenutno imamo
                    * dodavanje operatera -> za dodavanje operatera treba dodati informafije o ipAdresi i portu
                
                Kada se ovo odrati poziva se sledece: 

                IOperatorConnection proxy = Client.GetOperatorProxy(ip, port);
                proxy.UpdateStatus(myUsernameOnOperator, value);
            */

            return retVal;
        }
    }
}
