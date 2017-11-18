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
        private IBankConnection bankProxy;

        public void BankToOperator()
        {
            throw new NotImplementedException();
        }

        public void ClientToBankAddAccount(User u)
        {
            //pozovem metodu iz banke
            //kacimo se na banku, uplacujemo novac
            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }

            bankProxy.AddAccount(u);
        }

        public bool ClientToBankTransfer(string myUsername, string myUsernameOnOperator, string operatorUsername, int value)
        {
            bool retVal;

            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }
            retVal = bankProxy.Transfer(myUsername, myUsernameOnOperator, operatorUsername, value);
            Console.WriteLine("Client to bank...");
            return retVal;
        }

        public User ClientToBankCheckLogin(string username, string password)
        {
            
            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }
            Console.WriteLine(username);
           
            User u=bankProxy.CheckLogin(username, password);
            return u;

        }

        public void OperatorToClient()
        {
            throw new NotImplementedException();
        }

      
    }
}
