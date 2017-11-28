using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Metoda
    {
        public string NazivMetode { get; set; }
        public int BrojPoziva { get; set; }
        public string NazivServisa { get; set; }

        public Metoda(string naziv, int brPoziva, string nazivServisa)
        {
           this.NazivMetode = naziv;
           this.BrojPoziva = brPoziva;
           this.NazivServisa = nazivServisa;
        }
        public Metoda() { }
    }
}
