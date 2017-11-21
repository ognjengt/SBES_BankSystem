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
            string usernameDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Username), "kljuc");
            string brojRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.BrojRacuna), "kljuc");
            string operatorDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Operater), "kljuc");
            string tipRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.TipRacuna), "kljuc");

            Racun desifrovan = new Racun();
            desifrovan.BrojRacuna = brojRacunaDesifrovan;
            desifrovan.Operater = operatorDesifrovan;
            desifrovan.StanjeRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.StanjeRacuna), "kljuc");
            desifrovan.TipRacuna = tipRacunaDesifrovan;
            desifrovan.Username = usernameDesifrovan;



            OperatorDB.BazaRacuna.Add(desifrovan.BrojRacuna, desifrovan);
            Console.WriteLine("Dodat novi racun: " + desifrovan.BrojRacuna + " " + desifrovan.Username);
            return true;
        }

        public bool UpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, string suma)
        {
            string desifrovanKorisnikKojiJeUplatio = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(korisnikKojiJeUplatio), "kljuc");
            string desifrovanOperatorKomeJeUplaceno = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(operaterKomeJeUplaceno), "kljuc");
            string desifrovanaSuma = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(suma), "kljuc");

            foreach (var racun in OperatorDB.BazaRacuna)
            {
                if (racun.Value.Username == desifrovanKorisnikKojiJeUplatio)
                {
                    int trenutnoStanjeRacuna = Int32.Parse(OperatorDB.BazaRacuna[racun.Value.BrojRacuna].StanjeRacuna) - Int32.Parse(desifrovanaSuma);
                    OperatorDB.BazaRacuna[racun.Value.BrojRacuna].StanjeRacuna = trenutnoStanjeRacuna.ToString();
                    return true;
                }
            }
            return false;
        }
    }
}
