using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("+-+-+-+-+-+-+-+");
            Console.WriteLine("|G|A|T|E|W|A|Y|");
            Console.WriteLine("+-+-+-+-+-+-+-+");
            Console.WriteLine();
            //GatewayServer server = new GatewayServer();
            //server.Start();

            // TEST
            //GatewayLogger.AddMethod("Transfer", "Bank");
            //GatewayLogger.AddMethod("Transfer", "Bank");
            //GatewayLogger.AddMethod("Transfer", "Bank");
            //GatewayLogger.AddMethod("Transfer", "Bank");

            //GatewayLogger.AddMethod("UpdateStatus", "Telenor");
            //GatewayLogger.AddMethod("UpdateStatus", "Telenor");
            //GatewayLogger.AddMethod("UpdateStatus", "Telenor");

            //GatewayLogger.AddMethod("NekaTamo", "VIP");
            //GatewayLogger.AddMethod("NekaTamo", "VIP");

            //GatewayLogger.AddInicijator("Ognjen", "192.168.1.1", 2000);
            //GatewayLogger.AddInicijator("Ognjen", "192.168.1.1", 2000);

            //GatewayLogger.AddInicijator("Sofija", "2000", 2000);
            //GatewayLogger.AddInicijator("Sofija", "2000", 2000);
            //GatewayLogger.AddInicijator("Sofija", "2000", 2000);
            //GatewayLogger.UcitajStatistikuMetoda();
            //GatewayLogger.UcitajStatistikuInicijatora();

            //GatewayLogger.GenerisiIzvestaj();

            Console.ReadKey();
            Server2<IGatewayConnection> server = new Server2<IGatewayConnection>(IPFinder.GetIPAddress(),Konstante.GATEWAY_PORT.ToString(),"GatewayConnection",typeof(GatewayConnection));

            GatewayLogger.UcitajStatistikuMetoda();
            GatewayLogger.UcitajStatistikuInicijatora();


            Console.ReadKey();
            server.Close();
        }
    }
}
