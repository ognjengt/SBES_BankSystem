using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Racun
    {
        public User Korisnik { get; set; }
        public string BrojRacuna { get; set; }

        public Racun() { }
        public Racun(User korisnik,string brRacuna) {

            this.Korisnik = korisnik;
            this.BrojRacuna = brRacuna;
        }

    }
}
