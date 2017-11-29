using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class IPFinder
    {
        public static string GetIPAddress()
        {
            //IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            //IPAddress ipAddress = ipHostInfo.AddressList
            //    .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
            //return ipAddress.ToString();

            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
    }
}
