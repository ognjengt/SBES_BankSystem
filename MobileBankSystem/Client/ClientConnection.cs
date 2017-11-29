using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;
using System.Threading;
using Common;

namespace Client
{
    public class ClientConnection : IClientConnection
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SendBill(string suma)
        {

            KlientskiRacun.dugNaOperatoru += Int32.Parse(suma);

            Console.WriteLine("Vas operater je poslao mesecni izvestaj, dug za ovaj mesec iznosi: " + suma + ". Vas ukupan dug iznosi: " + KlientskiRacun.dugNaOperatoru);
        }
    }
}
