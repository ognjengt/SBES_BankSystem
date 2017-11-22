using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using System.Diagnostics;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prvo konekcija na server radi pristupanja bazi podataka ( gde admin postoji?)
            // Prvo autentifikacija, u zavisnosti od toga gleda se da li je admin ili ne (iz nekog txt-a)
            // ???
            string kljuc = "kljuc";
            Common.Client cli = new Common.Client();
            IGatewayConnection gatewayProxy = cli.GetGatewayProxy();
            Console.WriteLine("Username:");
            string user = Console.ReadLine();
            string userSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(user, kljuc));
            

            Console.WriteLine("Password:");
            string pass = Console.ReadLine();
            string passSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(pass, kljuc));


            User ulogovanUser = gatewayProxy.ClientToBankCheckLogin(userSifrovano, passSifrovano, "client");
            if (ulogovanUser != null)
            {
                string userSifrovanoZaRacun = BitConverter.ToString(Sifrovanje.sifrujCBC(ulogovanUser.Username, kljuc));

                Console.WriteLine("Uspesno logovanje " + ulogovanUser.Username);
                KlientskiRacun.racun = gatewayProxy.ClientToBankUzmiKlijentskiRacun(userSifrovanoZaRacun);
                if (KlientskiRacun.racun == null) {

                    Console.WriteLine("Ne postoji klijentski racun ");

                } else {

                    Console.WriteLine("Klijentski racun:" + KlientskiRacun.racun.BrojRacuna);

                }

                // podici server za klijenta i javiti banci sa metodomSetIpAndPort na kom portu i ip adresi slusa

                if (ulogovanUser.Uloga == "admin")
                {
                    MeniAdmin(gatewayProxy);


                }
                else if (ulogovanUser.Uloga == "korisnik") {
                    MeniKorisnik(gatewayProxy);
                }
            }
            else
            {
                Console.WriteLine("Neuspesno logovanje");
            }

            Console.ReadKey();
            gatewayProxy.ClientToBankShutdownClient(ulogovanUser.Username);
        }

        private static void MeniAdmin(IGatewayConnection gatewayProxy) {

            int izbor;
            do {

                Console.WriteLine("1. Dodavanje korisnika/operatera");
                Console.WriteLine("2. Kreiranje novog racuna");
                Console.WriteLine("3. Brisanje racuna");
                Console.WriteLine("4. Izmena racuna");
                Console.WriteLine("5. Dodaj korisnika(performance test)");
                Console.WriteLine("6. Ispis statistike sistema");
                Console.WriteLine("0.Izlaz");
                Console.WriteLine("Izaberi jedan od ponudjenih");
                izbor = Int32.Parse(Console.ReadLine());

                //switch za izbor

                switch (izbor) {

                    case 1:
                        DodajKorisnika(gatewayProxy,1);
                        break;
                    case 2:
                        KreirajRacun(gatewayProxy);
                        break;
                    case 3:
                        ObrisiRacun(gatewayProxy);
                        break;
                    case 4:
                        IzmeniRacun(gatewayProxy);
                        break;
                    case 5:
                        DodajKorisnikaTest(gatewayProxy);
                        break;
                    case 6:
                        GatewayLogger.GenerisiIzvestaj();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Ne postoji ta opcija");
                        break;
                }

            } while (izbor != 0);

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
                switch (izbor)
                {
                    case 1:
                        Transferuj(gatewayProxy);
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }

            } while (izbor != 0);

        }

        private static void DodajKorisnika(IGatewayConnection gatewayProxy,int mode)
        {
            if (mode == 1)
            {
                Console.WriteLine("Korisnicko ime:");
                string username = Console.ReadLine();

                Console.WriteLine("Lozinka:");
                string lozinka = Console.ReadLine();

                Console.WriteLine("Uloga:");
                string uloga = Console.ReadLine();

                //pravljenje korisnika
                User noviUser = new User();
                noviUser.Username = BitConverter.ToString(Sifrovanje.sifrujCBC(username, "kljuc"));
                noviUser.Password = BitConverter.ToString(Sifrovanje.sifrujCBC(lozinka, "kljuc"));
                noviUser.Uloga = BitConverter.ToString(Sifrovanje.sifrujCBC(uloga, "kljuc"));

                gatewayProxy.ClientToBankAddAccount(noviUser,mode);
            }

            else
            {
                Console.WriteLine("Korisnicko ime:");
                string username = Console.ReadLine();

                Console.WriteLine("Lozinka:");
                string lozinka = Console.ReadLine();

                Console.WriteLine("Uloga:");
                string uloga = Console.ReadLine();

                //pravljenje korisnika
                User noviUser = new User();
                noviUser.Username = BitConverter.ToString(Sifrovanje.sifrujECB(username, "kljuc"));
                noviUser.Password = BitConverter.ToString(Sifrovanje.sifrujECB(lozinka, "kljuc"));
                noviUser.Uloga = BitConverter.ToString(Sifrovanje.sifrujECB(uloga, "kljuc"));

                gatewayProxy.ClientToBankAddAccount(noviUser,mode);
            }

        }

        private static void KreirajRacun(IGatewayConnection gatewayProxy)
        {
          
                Console.WriteLine("Korisnicko ime vlasnika racuna (fizicko ili pravno lice): ");
                string username = Console.ReadLine();
                string userSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(username, "kljuc"));



                Console.WriteLine("Broj racuna: ");
                string brojRacuna = Console.ReadLine();
                string brojRacunaSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(brojRacuna, "kljuc"));



                Console.WriteLine("Tip racuna (fizicki ili pravni): ");
                string tipRacuna = Console.ReadLine();
                string tipRacunaSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(tipRacuna, "kljuc"));



                string operater = "null";
                string operaterSifrovano = "null";
                if (tipRacuna == "fizicki")
                {
                    Console.WriteLine("Korisnicko ime naloga operatera: ");
                    operater = Console.ReadLine();
                    operaterSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(operater, "kljuc"));

                }

                Console.WriteLine("Inicijalno stanje ");
                string stanje = Console.ReadLine();
                string stanjeSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(stanje, "kljuc"));



                Racun noviRacun = new Racun(userSifrovano, brojRacunaSifrovano, stanjeSifrovano, tipRacunaSifrovano, operaterSifrovano);

                var uspesnoKreiran = gatewayProxy.ClientToBankKreirajRacun(noviRacun);
                if (uspesnoKreiran == null)
                {
                    Console.WriteLine("Neuspesno kreiran racun, proverite da li ovaj broj racuna vec postoji, ili da korisnik na koga se dodaje ne postoji");
                }
                else
                {
                    Console.WriteLine("Uspesno kreiran racun!");
                }
        }


        private static void DodajKorisnikaTest(IGatewayConnection gatewayProxy)
        {
            Console.WriteLine("Performance testing...");
            Console.WriteLine("3DES CBC: ");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DodajKorisnika(gatewayProxy,1);
            sw.Stop();
            Console.WriteLine("Elapsed={0} ", sw.Elapsed);
            Console.WriteLine("=================================================");
            Console.WriteLine("3DES ECB: ");
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            DodajKorisnika(gatewayProxy,2);
            sw1.Stop();
            Console.WriteLine("Elapsed={0} ", sw1.Elapsed);
        }

        private static void ObrisiRacun(IGatewayConnection gatewayProxy)
        {
            Console.WriteLine("Broj racuna koji zelite da obrisete: ");
            string brRacuna = Console.ReadLine();

            string sifrovanRacun = BitConverter.ToString(Sifrovanje.sifrujCBC(brRacuna, "kljuc"));

            if (gatewayProxy.ClientToBankObrisiRacun(sifrovanRacun))
            {
                Console.WriteLine("Racun uspesno obrisan!");
            }
            else
            {
                Console.WriteLine("Racun nije uspesno obrisan, proverite da li postoji taj broj racuna");
            }
        }

        private static void IzmeniRacun(IGatewayConnection gatewayProxy)
        {
            Console.WriteLine("Izmena .... todo");
        }

        private static void Transferuj(IGatewayConnection gatewayProxy)
        {
            Console.WriteLine("Unesite broj operatorskog racuna na koji zelite da uplatite: ");
            string brojRacuna = Console.ReadLine();
            string sifrovanBrojOperatorskogRacuna = BitConverter.ToString(Sifrovanje.sifrujCBC(brojRacuna, "kljuc"));

            Console.WriteLine("Unesite sumu: ");
            string suma = Console.ReadLine();
            string sifrovanaSuma = BitConverter.ToString(Sifrovanje.sifrujCBC(suma, "kljuc"));


            string brojKlijentskogRacunaSifrovan = BitConverter.ToString(Sifrovanje.sifrujCBC(KlientskiRacun.racun.BrojRacuna, "kljuc"));
            string JASIFROVAN = BitConverter.ToString(Sifrovanje.sifrujCBC(KlientskiRacun.racun.Username, "kljuc"));

            gatewayProxy.ClientToBankTransfer(brojKlijentskogRacunaSifrovan, sifrovanBrojOperatorskogRacuna, JASIFROVAN, sifrovanaSuma);

        }
    }
}
