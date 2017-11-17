using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway
{
    public class GatewayConnection : IGatewayConnection
    {
        public void BankToOperator()
        {
            throw new NotImplementedException();
        }

        public void ClientToBank()
        {
            //pozovem metodu iz banke
            //kacimo se na banku, uplacujemo novac
            Console.WriteLine("Client to bank...");
        }

        public void OperatorToClient()
        {
            throw new NotImplementedException();
        }
    }
}
