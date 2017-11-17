using Common;
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

        public void ClientToBankAddAccount(User u)
        {
            //pozovem metodu iz banke
            //kacimo se na banku, uplacujemo novac
            Console.WriteLine("Client to bank...");
        }

        public void ClientToBankTransfer()
        {
            //pozovem metodu iz banke
            //kacimo se na banku, uplacujemo novac
            Console.WriteLine("Client to bank...");
        }
        public bool ClientToBankCheckLogin(string username, string password)
        {
            return true;
        }

        public void OperatorToClient()
        {
            throw new NotImplementedException();
        }

      
    }
}
