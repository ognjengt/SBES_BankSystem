using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer : Server
    {
        public override void Close()
        {
            serviceHost.Close();
        }

        public override void Start()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:65000/BankConnection";

            serviceHost = new ServiceHost(typeof(BankConnetion));
            serviceHost.AddServiceEndpoint(typeof(IBankConnection), binding, address);

            serviceHost.Open();
            this.ipAddress = "localhost"; // kasnije izmeniti
            this.port = 65000;

        }
    }
}
