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
        private ServiceHost serviceHost = null;

        public abstract void Start();
        public abstract void Close();
    }
}
