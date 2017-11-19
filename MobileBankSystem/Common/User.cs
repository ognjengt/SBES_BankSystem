using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Uloga { get; set; } // admin, korisnik ili operater
        public string IpAddress { get; set; }
        public int Port { get; set; }


        public User(string username, string password,string uloga, string ipAddress, int port) {

            this.Username = username;
            this.Password = password;
            this.Uloga = uloga;
            this.IpAddress = ipAddress;
            this.Port = port;
        }
        public User()
        {

        }
      
    }

   
}
