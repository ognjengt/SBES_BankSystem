using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    // Potrebno je dodati polja koja su zajednicka i za pravna i za fizicka lica
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Uloga { get; set; }
        public string BrojRacuna { get; set; }


        public User(string username, string password,string uloga,string brRacuna ) {

            this.Username = username;
            this.Password = password;
            this.Uloga = uloga;
            this.BrojRacuna = brRacuna;
        }
        public User()
        {

        }
    }

   
}
