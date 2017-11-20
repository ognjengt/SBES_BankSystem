using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Operator
{
    public class OperaterConnection : IOperatorConnection
    {

        public bool NotifyRacunAdded(Racun r)
        {
            OperatorDB.BazaRacuna.Add(r.BrojRacuna, r);
            Console.WriteLine("Dodat novi racun: " + r.BrojRacuna + " " + r.Username);
            return true;
        }

        public bool UpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, int suma)
        {
            foreach (var racun in OperatorDB.BazaRacuna)
            {
                if (racun.Value.Username == korisnikKojiJeUplatio)
                {
                    OperatorDB.BazaRacuna[racun.Value.BrojRacuna].StanjeRacuna -= suma;
                    return true;
                }
            }
            return false;
        }
    }
}
