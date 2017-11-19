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
        public void AddClient()
        {
            Console.WriteLine("Client added");
        }

        public bool NotifyRacunAdded(Racun r)
        {
            OperatorDB.BazaRacuna.Add(r.BrojRacuna, r);
            Console.WriteLine("Dodat novi racun: " + r.BrojRacuna + " " + r.Username);
            return true;
        }

        public void UpdateStatus()
        {
            Console.WriteLine("StatusUpdated");
        }
    }
}
