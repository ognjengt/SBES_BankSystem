using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class User
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
        public string Uloga { get; set; }
        public string BrojRacuna { get; set; }


        public User(string username, SecureString password,string uloga,string brRacuna ) {

            this.Username = username;
            this.Password = password;
            this.Uloga = uloga;
            this.BrojRacuna = brRacuna;
        }
    }

   
}
