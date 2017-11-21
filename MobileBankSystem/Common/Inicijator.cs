using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Inicijator
    {
        public string Username { get; set; }
        public int BrojPoziva { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }

        public Inicijator(string username, int brPoziva, string ip, int port)
        {
            Username = username;
            BrojPoziva = brPoziva;
            IpAddress = ip;
            Port = port;
        }
        public Inicijator() { }
    }
}
