using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class UserIRacun
    {
        public User Korisnik { get; set; }
        public Racun Racun { get; set; }

        public UserIRacun()
        {

        }
        public UserIRacun(User u, Racun r)
        {
            this.Korisnik = u;
            this.Racun = r;
        }
    }
}
