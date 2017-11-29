using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using System.Diagnostics;
using System.Security.Principal;

namespace Client
{
    class Program
    {
        /*
         
             */
        static void Main(string[] args)
        {
            Console.WriteLine("+-+-+-+-+-+-+");
            Console.WriteLine("|C|L|I|E|N|T|");
            Console.WriteLine("+-+-+-+-+-+-+");
            Console.WriteLine();
            // Prvo konekcija na server radi pristupanja bazi podataka ( gde admin postoji?)
            // Prvo autentifikacija, u zavisnosti od toga gleda se da li je admin ili ne (iz nekog txt-a)
            // ???
            string kljuc = Konstante.ENCRYPTION_KEY;
            Client<IGatewayConnection> cli = new Client<IGatewayConnection>("mbgateway", Konstante.GATEWAY_IP, Konstante.GATEWAY_PORT.ToString(), "GatewayConnection");
            IGatewayConnection gatewayProxy = cli.GetProxy();
            Console.WriteLine(">Login");
            User ulogovanUser = new User();
            bool uspesnoLogovanje = false;

            while (!uspesnoLogovanje)
            {
                Console.WriteLine("Username:");
                string user = Console.ReadLine();
                string userSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(user, kljuc));


                Console.WriteLine("Password:");
                string pass = "";
                ConsoleKeyInfo key;

                do
                {
                    key = Console.ReadKey(true);

                    // Backspace Should Not Work
                    if (key.Key != ConsoleKey.Backspace)
                    {
                        pass += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write("\b");
                    }
                }
                // Stops Receving Keys Once Enter is Pressed
                while (key.Key != ConsoleKey.Enter);
                pass = pass.Replace("\r", "");
                string passSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(pass, kljuc));

                ulogovanUser = gatewayProxy.ClientToBankCheckLogin(userSifrovano, passSifrovano, "client");
                if (ulogovanUser != null)
                {
                    uspesnoLogovanje = true;
                }
                else
                {
                    Console.WriteLine("\nNeuspesno logovanje");
                }
            }
            
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
                //ClientServer server = new ClientServer();
                //server.Start();
                //string sifrovanUsername = BitConverter.ToString(Sifrovanje.sifrujCBC(ulogovanUser.Username, Konstante.ENCRYPTION_KEY));
                //gatewayProxy.ClientToBankSetIpAndPortClient(sifrovanUsername, server.ipAddress, server.port.ToString());

                Server2<IClientConnection> server = new Server2<IClientConnection>(IPFinder.GetIPAddress(), Konstante.INITIAL_CLIENT_PORT.ToString(), "ClientConnection", typeof(ClientConnection));
                string sifrovanUsername = BitConverter.ToString(Sifrovanje.sifrujCBC(ulogovanUser.Username, Konstante.ENCRYPTION_KEY));
                gatewayProxy.ClientToBankSetIpAndPortClient(sifrovanUsername, server.ipAddress, server.connectedPort.ToString());

                // Javi gatewayu da te doda u listu instanci
                gatewayProxy.CheckIntoGateway(server.ipAddress, server.connectedPort.ToString(), CertManager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

                if (ulogovanUser.Uloga == "admin")
                {
                    MeniAdmin(gatewayProxy);
                }
                else if (ulogovanUser.Uloga == "korisnik") {
                    MeniKorisnik(gatewayProxy);
                }
            }

            Console.ReadKey();
            gatewayProxy.ClientToBankShutdownClient(ulogovanUser.Username);
        }

        private static void MeniAdmin(IGatewayConnection gatewayProxy) {

            int izbor;
            do {
                Console.WriteLine();
                Console.WriteLine("+-+-+-+-+-+-+");
                Console.WriteLine("|O|P|C|I|J|E|");
                Console.WriteLine("+-+-+-+-+-+-+");
                Console.WriteLine("1. Dodavanje korisnika/operatera");
                Console.WriteLine("2. Kreiranje novog racuna");
                Console.WriteLine("3. Brisanje racuna");
                Console.WriteLine("4. Izmena racuna");
                Console.WriteLine("5. Dodaj korisnika(performance test)");
                Console.WriteLine("6. Ispis statistike sistema");
                Console.WriteLine("0. Izlaz");
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
                Console.WriteLine();
                Console.WriteLine("+-+-+-+-+-+-+");
                Console.WriteLine("|O|P|C|I|J|E|");
                Console.WriteLine("+-+-+-+-+-+-+");
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
            Console.WriteLine();
            Console.WriteLine(">Dodavanje korisnika:");
            if (mode == 1)
            {
                Console.WriteLine("Korisnicko ime:");
                string username = Console.ReadLine();

                Console.WriteLine("Lozinka:");
                string lozinka = Console.ReadLine();

                Console.WriteLine("Uloga:");

                string uloga;
                do
                {
                    Console.WriteLine("Unesi ulogu(korisnik,admin ili operater):");
                    uloga = Console.ReadLine();
                }
                while (uloga != "korisnik" && uloga != "admin" && uloga != "operater");

                //pravljenje korisnika
                User noviUser = new User();
                noviUser.Username = BitConverter.ToString(Sifrovanje.sifrujCBC(username, Konstante.ENCRYPTION_KEY));
                noviUser.Password = BitConverter.ToString(Sifrovanje.sifrujCBC(lozinka, Konstante.ENCRYPTION_KEY));
                noviUser.Uloga = BitConverter.ToString(Sifrovanje.sifrujCBC(uloga, Konstante.ENCRYPTION_KEY));

                bool dodao = gatewayProxy.ClientToBankAddAccount(noviUser,mode);
                if (dodao)
                {
                    Console.WriteLine("Uspesno dodat korisnik!");
                }
                else
                {
                    Console.WriteLine("Neuspesno dodavanje korisnika!");
                }
            }

            else
            {
                Console.WriteLine("Korisnicko ime:");
                string username = Console.ReadLine();

                Console.WriteLine("Lozinka:");
                string lozinka = Console.ReadLine();

                Console.WriteLine("Uloga:");
                string uloga;
                do
                {
                    Console.WriteLine("Unesi ulogu(korisnik,admin ili operater):");
                    uloga = Console.ReadLine();
                }
                while (uloga != "korisnik" && uloga != "admin" && uloga != "operater");

                //pravljenje korisnika
                User noviUser = new User();
                noviUser.Username = BitConverter.ToString(Sifrovanje.sifrujECB(username, Konstante.ENCRYPTION_KEY));
                noviUser.Password = BitConverter.ToString(Sifrovanje.sifrujECB(lozinka, Konstante.ENCRYPTION_KEY));
                noviUser.Uloga = BitConverter.ToString(Sifrovanje.sifrujECB(uloga, Konstante.ENCRYPTION_KEY));
                bool addedOnBank = gatewayProxy.ClientToBankAddAccount(noviUser, mode);
                if (addedOnBank)
                {
                    Console.WriteLine("Uspesno dodat korisnik!");
                }
                else
                {
                    Console.WriteLine("Neuspesno dodavanje korisnika!");
                }

            }

        }

        private static void KreirajRacun(IGatewayConnection gatewayProxy)
        {
                Console.WriteLine();
                Console.WriteLine(">Dodaj racun:");
                Console.WriteLine("Korisnicko ime vlasnika racuna: ");
                string username = Console.ReadLine();
                string userSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(username, Konstante.ENCRYPTION_KEY));



                Console.WriteLine("Broj racuna: ");
                string brojRacuna = Console.ReadLine();
                string brojRacunaSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(brojRacuna, Konstante.ENCRYPTION_KEY));

            string tipRacuna;
            do
            {
                Console.WriteLine("Tip racuna (fizicki ili pravni):");
                tipRacuna = Console.ReadLine();
            }
            while (tipRacuna != "fizicki" && tipRacuna!="pravni");

            string tipRacunaSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(tipRacuna, Konstante.ENCRYPTION_KEY));



            string operater = "null";
            string operaterSifrovano = "null";
            if (tipRacuna == "fizicki")
            {
                Console.WriteLine("Korisnicko ime naloga operatera: ");
                operater = Console.ReadLine();
            }
            operaterSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(operater, Konstante.ENCRYPTION_KEY));

            Console.WriteLine("Inicijalno stanje(broj):");
            string stanje = Console.ReadLine();
            string stanjeSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(stanje, Konstante.ENCRYPTION_KEY));



            Racun noviRacun = new Racun(userSifrovano, brojRacunaSifrovano, stanjeSifrovano, tipRacunaSifrovano, operaterSifrovano);

            var uspesnoKreiran = gatewayProxy.ClientToBankKreirajRacun(noviRacun);

            if (uspesnoKreiran == null)
            {
                Console.WriteLine("Neuspesno kreiran racun, proverite da li ovaj broj racuna vec postoji, ili da korisnik na koga se dodaje ne postoji");
            }
            else
            {
                Console.WriteLine("Uspesno kreiran racun na banci!");
            }


            if (tipRacuna == "fizicki")
            {
                User operaterZaProsledjivanje = gatewayProxy.ClientToBankGetOperator(operaterSifrovano);
                User desifrovanOperaterZaProsledjivanje = Sifrovanje.desifrujUsera(operaterZaProsledjivanje);
                if (gatewayProxy.ClientToOperatorAddRacun(noviRacun, desifrovanOperaterZaProsledjivanje.IpAddress, desifrovanOperaterZaProsledjivanje.Port))
                {
                    Console.WriteLine("Uspesno kreiran racun na operateru");
                }
                else
                {
                    Console.WriteLine("Neuspesno kreiran racun na operateru");
                }
            }

        }


        private static void DodajKorisnikaTest(IGatewayConnection gatewayProxy)
        {
            Console.WriteLine();
            Console.WriteLine(">Performance testing:");
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
            Console.WriteLine();
            Console.WriteLine(">Brisanje racuna:");
            Console.WriteLine("Broj racuna koji zelite da obrisete: ");
            string brRacuna = Console.ReadLine();

            string sifrovanRacun = BitConverter.ToString(Sifrovanje.sifrujCBC(brRacuna, Konstante.ENCRYPTION_KEY));

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
            Console.WriteLine("> Izmena racuna: ");
            Console.WriteLine("> Broj racuna koji zelite da izmenite: ");
            string brojRacuna = Console.ReadLine();
            Console.WriteLine("> Novo stanje racuna: ");
            string stanje = Console.ReadLine();

            Racun r = new Racun();
            r.BrojRacuna = brojRacuna;
            r.StanjeRacuna = stanje;

            Racun sifrovan = Sifrovanje.sifrujRacun(r);

            if (gatewayProxy.ClientToBankIzmeniRacun(sifrovan))
            {
                Console.WriteLine("Racun uspesno izmenjen");
            }
            else
            {
                Console.WriteLine("Racun neuspesno izmenjen, proverite da li postoji taj broj racuna");
            }

        }

        private static void Transferuj(IGatewayConnection gatewayProxy)
        {
            Console.WriteLine();
            Console.WriteLine(">Transfer:");
            Console.WriteLine("Unesite broj operatorskog racuna na koji zelite da uplatite: ");
            string brojRacuna = Console.ReadLine();
            string sifrovanBrojOperatorskogRacuna = BitConverter.ToString(Sifrovanje.sifrujCBC(brojRacuna, Konstante.ENCRYPTION_KEY));

            Console.WriteLine("Unesite sumu: ");
            string suma = Console.ReadLine();
            string sifrovanaSuma = BitConverter.ToString(Sifrovanje.sifrujCBC(suma, Konstante.ENCRYPTION_KEY));


            string brojKlijentskogRacunaSifrovan = BitConverter.ToString(Sifrovanje.sifrujCBC(KlientskiRacun.racun.BrojRacuna, Konstante.ENCRYPTION_KEY));
            string JASIFROVAN = BitConverter.ToString(Sifrovanje.sifrujCBC(KlientskiRacun.racun.Username, Konstante.ENCRYPTION_KEY));

            gatewayProxy.ClientToBankTransfer(brojKlijentskogRacunaSifrovan, sifrovanBrojOperatorskogRacuna, JASIFROVAN, sifrovanaSuma);

        }
    }
}
