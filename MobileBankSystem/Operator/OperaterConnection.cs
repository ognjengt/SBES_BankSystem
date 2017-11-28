using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Xml.Serialization;
using System.IO;

namespace Operator
{
    public class OperaterConnection : IOperatorConnection
    {
        object locker = new object();
        public bool AddRacun(Racun r)
        {
            Racun desifrovanRacun = Sifrovanje.desifrujRacun(r);
            
            if (OperatorDB.BazaKorisnika.ContainsKey(desifrovanRacun.BrojRacuna))
            {
                return false;
            }
            OperatorskiRacun or = new OperatorskiRacun(desifrovanRacun.Username, OperatorDB.brRacuna++.ToString(), "0");
            OperatorDB.BazaRacuna.Add(or.BrojRacuna,or);

            upisiRacune(OperatorDB.operatorName);
            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //public bool NotifyRacunAdded(Racun r)
        //{
        //    string usernameDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Username), Konstante.ENCRYPTION_KEY);
        //    string brojRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.BrojRacuna), Konstante.ENCRYPTION_KEY);
        //    string operatorDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Operater), Konstante.ENCRYPTION_KEY);
        //    string tipRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.TipRacuna), Konstante.ENCRYPTION_KEY);
        
        //    Racun desifrovan = new Racun();
        //    desifrovan.BrojRacuna = brojRacunaDesifrovan;
        //    desifrovan.Operater = operatorDesifrovan;
        //    desifrovan.StanjeRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.StanjeRacuna), Konstante.ENCRYPTION_KEY);
        //    desifrovan.TipRacuna = tipRacunaDesifrovan;
        //    desifrovan.Username = usernameDesifrovan;

        //    OperatorDB.BazaRacuna.Add(desifrovan.BrojRacuna, desifrovan);
        //    Console.WriteLine("Dodat novi racun: " + desifrovan.BrojRacuna + " " + desifrovan.Username);
        //    return true;
        //}

        //public bool NotifyRacunChanged(Racun r)
        //{
        //    Racun desifrovan = Sifrovanje.desifrujRacun(r);

        //    OperatorDB.BazaRacuna[desifrovan.BrojRacuna] = desifrovan;
        //    return true;
        //}

        //public bool NotifyRacunDeleted(Racun r)
        //{
        //    Racun desifrovan = Sifrovanje.desifrujRacun(r);

        //    OperatorDB.BazaRacuna.Remove(desifrovan.BrojRacuna);
        //    return true;
        //}

        public bool UpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, string suma)
        {
            string desifrovanKorisnikKojiJeUplatio = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(korisnikKojiJeUplatio), Konstante.ENCRYPTION_KEY);
            string desifrovanOperatorKomeJeUplaceno = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(operaterKomeJeUplaceno), Konstante.ENCRYPTION_KEY);
            string desifrovanaSuma = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(suma), Konstante.ENCRYPTION_KEY);

            foreach (var racun in OperatorDB.BazaRacuna)
            {
                if (racun.Value.Username == desifrovanKorisnikKojiJeUplatio)
                {
                    int trenutnoStanjeRacuna = Int32.Parse(OperatorDB.BazaRacuna[racun.Value.BrojRacuna].Dug) - Int32.Parse(desifrovanaSuma);
                    OperatorDB.BazaRacuna[racun.Value.BrojRacuna].Dug = trenutnoStanjeRacuna.ToString();
                    return true;
                }
            }
            return false;
        }

        public void upisiKorisnike(string operatorname)
        {
            lock (locker)
            {
                string putanja = Environment.CurrentDirectory + "\\"+operatorname+"Korisnici.xml";

                List<User> listaKorisnika = new List<User>();
                //prepisati iz recnika u ovu listu
                foreach (User u in OperatorDB.BazaKorisnika.Values)
                {
                    User sifrovan = new User();
                    sifrovan = Sifrovanje.sifrujUsera(u);

                    listaKorisnika.Add(sifrovan);
                }

                XmlSerializer ser = new XmlSerializer(typeof(List<User>));

                StreamWriter sw = new StreamWriter(putanja);
                ser.Serialize(sw, listaKorisnika);

                sw.Close();
            }
        }

        public void upisiRacune(string operatorname)
        {
            lock (locker)
            {
                string putanja = Environment.CurrentDirectory + "\\" + operatorname + "Racuni.xml";

                List<OperatorskiRacun> listaRacuna = new List<OperatorskiRacun>();
                //prepisati iz recnika u ovu listu
                foreach (OperatorskiRacun r in OperatorDB.BazaRacuna.Values)
                {
                    OperatorskiRacun sifrovan = new OperatorskiRacun();
                    sifrovan = Sifrovanje.sifrujOperatorskiRacun(r);

                    listaRacuna.Add(sifrovan);
                }

                XmlSerializer ser = new XmlSerializer(typeof(List<User>));

                StreamWriter sw = new StreamWriter(putanja);
                ser.Serialize(sw, listaRacuna);

                sw.Close();
            }
        }


    }
}
