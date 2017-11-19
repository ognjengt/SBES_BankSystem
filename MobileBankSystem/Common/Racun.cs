using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Racun
    {
        public string Username { get; set; }
        public string BrojRacuna { get; set; }
        public int StanjeRacuna { get; set; }
        public string TipRacuna { get; set; }
        public string Operater { get; set; }

        public Racun() { }
        public Racun(string korisnickoIme,string brRacuna,int stanjeRacuna, string tipRacuna, string operater) {

            this.Username = korisnickoIme;
            this.BrojRacuna = brRacuna;
            this.StanjeRacuna = stanjeRacuna;
            this.TipRacuna = tipRacuna;
            this.Operater = operater;
        }

    }
}
