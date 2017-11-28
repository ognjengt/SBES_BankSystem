using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Uloga { get; set; } // admin, korisnik ili operater
        public string IpAddress { get; set; }
        public string Port { get; set; }

        //public string CN { get; set;}//cert name

        public User(string username, string password,string uloga, string ipAddress, string port) {

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
