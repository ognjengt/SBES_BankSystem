using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ListSerializer
    {
        public static string SerializeList(List<UserIRacun> lista)
        {
            string serialized = "";
            foreach (UserIRacun item in lista)
            {
                serialized += item.Korisnik.Username + ";" + item.Korisnik.IpAddress + ";" + item.Korisnik.Port + "/" + item.Racun.BrojRacuna + "|" + item.Racun.Username + "|" + item.Racun.StanjeRacuna + "|" + item.Racun.Operater+"}";
            }
            return serialized;
        }
        public static List<UserIRacun> DeserializeString(string serialized)
        {
            List<UserIRacun> deserializedList = new List<UserIRacun>();
            string[] celeKlase = serialized.Split('}');
            foreach (string item in celeKlase)
            {
                if (item == "")
                {
                    continue;
                }
                string[] korisnikIRacun = item.Split('/');
                string korisnikSerialized = korisnikIRacun[0];
                string racunSerialized = korisnikIRacun[1];

                string[] deloviKorisnika = korisnikSerialized.Split(';');
                string[] deloviRacuna = racunSerialized.Split('|');

                User korisnik = new User();
                korisnik.Username = deloviKorisnika[0];
                korisnik.IpAddress = deloviKorisnika[1];
                korisnik.Port = deloviKorisnika[2];

                Racun racun = new Racun();
                racun.BrojRacuna = deloviRacuna[0];
                racun.Username = deloviRacuna[1];
                racun.StanjeRacuna = deloviRacuna[2];
                racun.Operater = deloviRacuna[3];

                deserializedList.Add(new UserIRacun(korisnik, racun));

            }
            return deserializedList;
        }
    }
}
