using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Operator
{
    public class OperatorServer : Server
    {
        private int listeningPort = 64000; // definisano ovde zato sto moze biti vise operatora i treba svaki da slusa na drugom portu
        public override void Close()
        {
            serviceHost.Close();
        }

        public override void Start()
        {
            bool uspesnoStartovanje = false;
            do
            {
                NetTcpBinding binding = new NetTcpBinding();
                string address = "net.tcp://localhost:" + listeningPort + "/OperaterConnection";

                serviceHost = new ServiceHost(typeof(OperaterConnection));
                serviceHost.AddServiceEndpoint(typeof(IOperatorConnection), binding, address);
                try
                {
                    serviceHost.Open();
                    uspesnoStartovanje = true;
                }
                catch (Exception)
                {
                    uspesnoStartovanje = false;
                    listeningPort += 1;
                }
            } while (!uspesnoStartovanje);
            Console.WriteLine("Operater listening on port: "+listeningPort);
            this.ipAddress = "localhost"; // kasnije izmeniti
            this.port = listeningPort;
        }
    }
}
