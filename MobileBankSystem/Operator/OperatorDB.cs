using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator
{
    public static class OperatorDB
    {
        public static string operatorName;
        public static string brojOperatorskogRacuna;
        public static Dictionary<string, OperatorskiRacun> BazaRacuna = new Dictionary<string, OperatorskiRacun>();
        public static Dictionary<string, User> BazaKorisnika = new Dictionary<string, User>();
    }
}
