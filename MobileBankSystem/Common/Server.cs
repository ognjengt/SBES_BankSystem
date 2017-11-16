using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Server
    {
        private ServiceHost serviceHost = null;

        public void Start()
        {
            serviceHost = new ServiceHost(typeof(Connection));
            var binding = new NetTcpBinding();
            binding.TransactionFlow = true;
            serviceHost.AddServiceEndpoint(typeof(IConnection), binding, new
            Uri("net.tcp://localhost:6000/TransactionCoordinator"));
            serviceHost.Open();
            Console.WriteLine("Server ready and waiting for requests.");
        }
    }
}
