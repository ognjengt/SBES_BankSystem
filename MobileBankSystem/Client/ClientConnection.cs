using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interfaces;
using System.Threading;


namespace Client
{
    public class ClientConnection : IClientConnection
    {
        public void SendBill(string suma)
        {

            KlientskiRacun.racun.StanjeRacuna +=suma ;

            //ovde pozivamo transfer
        }
    }
}
