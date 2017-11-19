using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientServer : Server
    {
        private int listeningPort = 62000; // definisano ovde zato sto moze biti vise klijenata i treba svaki da slusa na drugom portu
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
                string address = "net.tcp://localhost:" + listeningPort + "/ClientConnection";

                serviceHost = new ServiceHost(typeof(ClientConnection));
                serviceHost.AddServiceEndpoint(typeof(IClientConnection), binding, address);
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
            Console.WriteLine("Client listening on port: " + listeningPort);
            this.ipAddress = "localhost"; // kasnije izmeniti
            this.port = listeningPort;
        }
    }
}
