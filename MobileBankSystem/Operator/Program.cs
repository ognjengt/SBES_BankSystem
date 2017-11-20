﻿using Common;
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

            Common.Client cli = new Common.Client();
            IGatewayConnection gatewayProxy = cli.GetGatewayProxy();
            bool uspesnoUlogovan = false;
            User ulogovanUser = new User();
            while (!uspesnoUlogovan)
            {
                Console.WriteLine("Username:");
                string user = Console.ReadLine();

                Console.WriteLine("Password:");
                string pass = Console.ReadLine();

                ulogovanUser = gatewayProxy.ClientToBankCheckLogin(user, pass, "operater");
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

            OperatorServer server = new OperatorServer();
            server.Start();

            // Javi banci na kom ip-u i portu slusas
            if(!gatewayProxy.ClientAndOperatorToBankSetIpAndPort(ulogovanUser.Username, server.ipAddress, server.port))
            {
                // Ukoliko vec postoji instanca tipa telenora, ugasi aplikaciju ili ponovo loguj itd...
                Environment.Exit(0);
            }



            Console.ReadKey();
        }

        private static void SendBill(IGatewayConnection proxy)
        {

            while (true) {

                foreach (var korisnik in OperatorDB.BazaKorisnika) // TREBA NAPUNITI BAZU KORISNIKA
                {
                    Random r = new Random();
                    int randomSuma = r.Next(100,1000);
                    proxy.OperatorToClientSendBill(randomSuma,korisnik.Value.IpAddress,korisnik.Value.Port);


                }

                Thread.Sleep(20000);// 2 minuta ustvari
            }
        }
    }
}
