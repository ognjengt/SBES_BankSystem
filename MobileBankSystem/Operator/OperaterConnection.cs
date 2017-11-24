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
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool NotifyRacunAdded(Racun r)
        {
            string usernameDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Username), Konstante.ENCRYPTION_KEY);
            string brojRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.BrojRacuna), Konstante.ENCRYPTION_KEY);
            string operatorDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Operater), Konstante.ENCRYPTION_KEY);
            string tipRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.TipRacuna), Konstante.ENCRYPTION_KEY);
        
            Racun desifrovan = new Racun();
            desifrovan.BrojRacuna = brojRacunaDesifrovan;
            desifrovan.Operater = operatorDesifrovan;
            desifrovan.StanjeRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.StanjeRacuna), Konstante.ENCRYPTION_KEY);
            desifrovan.TipRacuna = tipRacunaDesifrovan;
            desifrovan.Username = usernameDesifrovan;

            OperatorDB.BazaRacuna.Add(desifrovan.BrojRacuna, desifrovan);
            Console.WriteLine("Dodat novi racun: " + desifrovan.BrojRacuna + " " + desifrovan.Username);
            return true;
        }

        public bool UpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, string suma)
        {
            string desifrovanKorisnikKojiJeUplatio = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(korisnikKojiJeUplatio), Konstante.ENCRYPTION_KEY);
            string desifrovanOperatorKomeJeUplaceno = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(operaterKomeJeUplaceno), Konstante.ENCRYPTION_KEY);
            string desifrovanaSuma = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(suma), Konstante.ENCRYPTION_KEY);

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
