using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public static class BankDB
    {
       public static Dictionary<string, Racun> BazaRacuna = new Dictionary<string, Racun>();
       public static Dictionary<string, User> BazaKorisnika = new Dictionary<string, User>();
       public static Dictionary<string, User> BazaAktivnihKorisnika = new Dictionary<string, User>();
       public static Dictionary<string, User> BazaAktivnihOperatera = new Dictionary<string, User>();
    }
}
