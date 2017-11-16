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
        public void SendBill()
        {
            while (true)
            {
                Thread.Sleep(2000);
                Console.WriteLine("Bill sent");
            }
        }
    }
}
