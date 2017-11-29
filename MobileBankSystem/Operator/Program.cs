using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Operator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("+-+-+-+-+-+-+-+-+");
            Console.WriteLine("|O|P|E|R|A|T|E|R|");
            Console.WriteLine("+-+-+-+-+-+-+-+-+");
            Console.WriteLine();
            Client<IGatewayConnection> cli = new Client<IGatewayConnection>("mbgateway", Konstante.GATEWAY_IP, Konstante.GATEWAY_PORT.ToString(), "GatewayConnection");
            IGatewayConnection gatewayProxy = cli.GetProxy();
            bool uspesnoUlogovan = false;
            User ulogovanUser = new User();

            while (!uspesnoUlogovan)
            {
                Console.WriteLine("Username:");
                string user = Console.ReadLine();
                string userSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(user, Konstante.ENCRYPTION_KEY));

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
                string passSifrovano = BitConverter.ToString(Sifrovanje.sifrujCBC(pass, Konstante.ENCRYPTION_KEY));

                ulogovanUser = gatewayProxy.ClientToBankCheckLogin(userSifrovano, passSifrovano, "operater");
                if (ulogovanUser != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Uspesno logovanje " + ulogovanUser.Username);
                    uspesnoUlogovan = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Neuspesno logovanje");
                    uspesnoUlogovan = false;
                }
            }
            Console.WriteLine("Korisnici koji su na ovom operateru:");
            OperatorDB.operatorName = ulogovanUser.Username;
            ucitajRacune(OperatorDB.operatorName);
            ucitajKorisnike(OperatorDB.operatorName);
            foreach (var item in OperatorDB.BazaKorisnika.Values)
            {
                Console.WriteLine(item.Username);
            }

            //OperatorServer server = new OperatorServer();
            //server.Start();

            Server2<IOperatorConnection> server = new Server2<IOperatorConnection>("localhost", Konstante.INITIAL_OPERATER_PORT.ToString(), "OperaterConnection", typeof(OperaterConnection));


            // Javi banci na kom ip-u i portu slusas
            string sifrovanUsername = BitConverter.ToString(Sifrovanje.sifrujCBC(ulogovanUser.Username, Konstante.ENCRYPTION_KEY));
            if (!gatewayProxy.ClientAndOperatorToBankSetIpAndPort(sifrovanUsername, server.ipAddress, server.connectedPort.ToString()))
            {
                // Ukoliko vec postoji instanca tipa telenora, ugasi aplikaciju ili ponovo loguj itd...
                Environment.Exit(0);
            }

            gatewayProxy.CheckIntoGateway(server.ipAddress, server.connectedPort.ToString(), CertManager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            // Iscitaj iz xml-a
           


            // Ako je sve proslo ok, uzmi bazu svih racuna i klijenata ciji je operater npr telenor
            //string serializedList = gatewayProxy.OperatorToBankGetOperatorsClients(sifrovanUsername);
            //List<UserIRacun> aktivniKorisnici = ListSerializer.DeserializeString(serializedList);

            //foreach (var item in aktivniKorisnici)
            //{
            //    OperatorDB.BazaRacuna.Add(item.Racun.BrojRacuna,item.Racun);
            //}

            // U novom threadu prodji kroz sve aktivne korisnike i pozovi im sendBill
            Thread sendBillThread = new Thread(() => SendBill(gatewayProxy, sifrovanUsername));
            sendBillThread.Start();

            Console.ReadKey();
            gatewayProxy.OperatorToBankShutdownOperator(ulogovanUser.Username);
        }

        private static void SendBill(IGatewayConnection proxy, string sifrovanUsername)
        {

            while (true) {
                // Svaka 2 minuta uzmi aktivne klijente i njima salji
                string serializedList = proxy.OperatorToBankGetOperatorsClients(sifrovanUsername);
                List<UserIRacun> aktivniKorisnici = ListSerializer.DeserializeString(serializedList);

                foreach (var userIRacun in aktivniKorisnici)
                {
                    Random r = new Random();
                    int randomSuma = r.Next(100,500);
                    proxy.OperatorToClientSendBill(randomSuma.ToString(), userIRacun.Korisnik.IpAddress, userIRacun.Korisnik.Port);
                }

                Thread.Sleep(1000*15);// 2 minuta ustvari
            }
        }
        private static void ucitajRacune(string operatorName)
        {
            try
            {
                string putanja = Environment.CurrentDirectory + "\\"+operatorName+"Racuni.xml";

                List<OperatorskiRacun> listaRacuna = new List<OperatorskiRacun>();
                XmlSerializer xs = new XmlSerializer(typeof(List<OperatorskiRacun>));
                StreamReader sr = new StreamReader(putanja);
                listaRacuna = (List<OperatorskiRacun>)xs.Deserialize(sr);
                sr.Close();
                foreach (OperatorskiRacun r in listaRacuna)
                {
                    OperatorskiRacun clean = Sifrovanje.desifrujOperatorskiRacun(r);
                    OperatorDB.BazaRacuna.Add(clean.BrojRacuna, clean);
                }
            }
            catch
            {

            }
        }

        private static void ucitajKorisnike(string operatorName)
        {
            try
            {
                string putanja = Environment.CurrentDirectory + "\\" + operatorName + "Korisnici.xml";

                List<User> listaUsera = new List<User>();
                XmlSerializer xs = new XmlSerializer(typeof(List<User>));
                StreamReader sr = new StreamReader(putanja);
                listaUsera = (List<User>)xs.Deserialize(sr);
                sr.Close();
                foreach (User u in listaUsera)
                {
                    User clean = Sifrovanje.desifrujUsera(u);
                    OperatorDB.BazaKorisnika.Add(clean.Username, clean);
                }
            }
            catch
            {

            }
        }

    }
}
