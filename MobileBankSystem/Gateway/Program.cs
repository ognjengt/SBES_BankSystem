using Common;
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
            GatewayServer server = new GatewayServer();
            server.Start();

            GatewayLogger.UcitajStatistikuMetoda();
            GatewayLogger.UcitajStatistikuInicijatora();

            Console.ReadKey();
        }
    }
}
