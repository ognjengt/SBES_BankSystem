using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gateway
{
    public class GatewayServer : Server
    {
        public override void Close()
        {
            serviceHost.Close();
        }

        public override void Start()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:63000/GatewayConnection";

            serviceHost = new ServiceHost(typeof(GatewayConnection));
            serviceHost.AddServiceEndpoint(typeof(IGatewayConnection), binding, address);

            serviceHost.Open();
        }
    }
}
