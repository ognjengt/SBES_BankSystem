using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Instance
    {
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public string CN { get; set; }

        public Instance(string ip,string port, string cn)
        {
            this.IpAddress = ip;
            this.Port = port;
            this.CN = cn;
        }
    }
}
