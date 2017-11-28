using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class OperatorskiRacun
    {
        public string Username { get; set; }
        public string BrojRacuna { get; set; }
        public string Dug { get; set; }

        public OperatorskiRacun() { }
        public OperatorskiRacun(string korisnickoIme, string brRacuna, string dug)
        { 
            this.Username = korisnickoIme;
            this.BrojRacuna = brRacuna;
            this.Dug = dug;   
        }
    }
}
