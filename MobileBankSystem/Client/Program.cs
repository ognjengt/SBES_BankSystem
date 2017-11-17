using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prvo konekcija na server radi pristupanja bazi podataka ( gde admin postoji?)
            // Prvo autentifikacija, u zavisnosti od toga gleda se da li je admin ili ne (iz nekog txt-a)
            // ???
            Common.Client cli = new Common.Client();
            IGatewayConnection gatewayProxy = cli.GetGatewayProxy();
            Console.WriteLine("Username:");
            string user = Console.ReadLine();

            Console.WriteLine("Password:");
            string pass = Console.ReadLine();

           User ulogovanUser=gatewayProxy.ClientToBankCheckLogin(user, pass);
            if (ulogovanUser != null)
            {
                Console.WriteLine("Uspesno logovanje " + ulogovanUser.Username);

                if (ulogovanUser.Uloga == "admin")
                {
                    MeniAdmin(gatewayProxy);


                }
                else if(ulogovanUser.Uloga=="korisnik") {
                    MeniKorisnik(gatewayProxy);
                }
            }
            else
            {
                Console.WriteLine("Neuspesno logovanje");
            }

            Console.ReadKey();
        }

        private static void MeniAdmin(IGatewayConnection gatewayProxy) {

            int izbor;
            do{
                
                Console.WriteLine("1.Dodavanje korisnika");
                Console.WriteLine("0.Izlaz");
                Console.WriteLine("Izaberi jedan od ponudjenih");
                izbor = Int32.Parse(Console.ReadLine());

                //switch za izbor


            } while(izbor!=0);

        }

        private static void MeniKorisnik(IGatewayConnection gatewayProxy)
        {

            int izbor;
            do
            {

                Console.WriteLine("1.Uplata ");
                Console.WriteLine("0.Izlaz");
                Console.WriteLine("Izaberi jedan od ponudjenih");
                izbor = Int32.Parse(Console.ReadLine());

                //switch za izbor


            } while (izbor != 0);

        }
    }
}
