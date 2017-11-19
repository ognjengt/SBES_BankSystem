using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class Server
    {
        public ServiceHost serviceHost = null;
        public string ipAddress = "";
        public int port = 0;

        public abstract void Start();
        public abstract void Close();
    }
}
