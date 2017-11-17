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

           bool proveraLogovanja=gatewayProxy.ClientToBankCheckLogin(user, pass);
            if (proveraLogovanja)
            {
                Console.WriteLine("Uspesno logovanje");
            }

            Console.ReadKey();
        }
    }
}
