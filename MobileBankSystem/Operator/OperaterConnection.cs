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
            string usernameDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.Username), "kljuc");
            string brojRacunaDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.BrojRacuna), "kljuc");
            string operatorDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.Operater), "kljuc");
            string tipRacunaDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.TipRacuna), "kljuc");

            Racun desifrovan = new Racun();
            desifrovan.BrojRacuna = brojRacunaDesifrovan;
            desifrovan.Operater = operatorDesifrovan;
            desifrovan.StanjeRacuna = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.StanjeRacuna), "kljuc");
            desifrovan.TipRacuna = tipRacunaDesifrovan;
            desifrovan.Username = usernameDesifrovan;



            OperatorDB.BazaRacuna.Add(desifrovan.BrojRacuna, desifrovan);
            Console.WriteLine("Dodat novi racun: " + desifrovan.BrojRacuna + " " + desifrovan.Username);
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
