using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway
{
    public class GatewayDB
    {
        public static Dictionary<string, Instance> CertDBClients = new Dictionary<string, Instance>();
        public static Dictionary<string, Instance> CertDBOperaters = new Dictionary<string, Instance>();
    }
}
