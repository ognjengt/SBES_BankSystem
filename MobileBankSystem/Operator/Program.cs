using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Operator
{
    class Program
    {
        static void Main(string[] args)
        {

            Client<IGatewayConnection> cli = new Client<IGatewayConnection>("mbgateway", Konstante.GATEWAY_IP, Konstante.GATEWAY_PORT.ToString(), "GatewayConnection");
            IGatewayConnection gatewayProxy = cli.GetProxy();
            bool uspesnoUlogovan = false;
            User ulogovanUser = new User();
            while (!uspesnoUlogovan)
            {
                Console.WriteLine("Username:");
                string user = Console.ReadLine();
                string userSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(user, "kljuc"));

                Console.WriteLine("Password:");
                string pass = Console.ReadLine();
                string passSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(pass, "kljuc"));

                ulogovanUser = gatewayProxy.ClientToBankCheckLogin(userSifrovano, passSifrovano, "operater");
                if (ulogovanUser != null)
                {
                    Console.WriteLine("Uspesno logovanje " + ulogovanUser.Username);
                    uspesnoUlogovan = true;
                }
                else
                {
                    Console.WriteLine("Neuspesno logovanje");
                    uspesnoUlogovan = false;
                }
            }

            //OperatorServer server = new OperatorServer();
            //server.Start();

            Server2<IOperatorConnection> server = new Server2<IOperatorConnection>(IPFinder.GetIPAddress(), Konstante.INITIAL_OPERATER_PORT.ToString(), "OperaterConnection", typeof(OperaterConnection));


            // Javi banci na kom ip-u i portu slusas
            string sifrovanUsername = BitConverter.ToString(Sifrovanje.sifrujCBC(ulogovanUser.Username, "kljuc"));
            if (!gatewayProxy.ClientAndOperatorToBankSetIpAndPort(sifrovanUsername, server.ipAddress, server.connectedPort.ToString()))
            {
                // Ukoliko vec postoji instanca tipa telenora, ugasi aplikaciju ili ponovo loguj itd...
                Environment.Exit(0);
            }

            // Ako je sve proslo ok, uzmi bazu svih racuna i klijenata ciji je operater npr telenor
            string serializedList = gatewayProxy.OperatorToBankGetOperatorsClients(sifrovanUsername);
            List<UserIRacun> aktivniKorisnici = ListSerializer.DeserializeString(serializedList);

            // U novom threadu prodji kroz sve aktivne korisnike i pozovi im sendBill

            Console.ReadKey();
            gatewayProxy.OperatorToBankShutdownOperator(ulogovanUser.Username);
        }

        private static void SendBill(IGatewayConnection proxy)
        {

            while (true) {

                foreach (var korisnik in OperatorDB.BazaKorisnika) // TREBA NAPUNITI BAZU KORISNIKA
                {
                    Random r = new Random();
                    int randomSuma = r.Next(100,1000);
                    proxy.OperatorToClientSendBill(randomSuma.ToString(),korisnik.Value.IpAddress,korisnik.Value.Port);
                }

                Thread.Sleep(20000);// 2 minuta ustvari
            }
        }
    }
}
