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

        public bool ClientToBankTransfer(string myUsername, string myUsernameOnOperator, string operatorUsername, string value)
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

        public User ClientToBankCheckLogin(string username, string password, string nacinLogovanja)
        {
            
            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }

            User u =bankProxy.CheckLogin(username, password, nacinLogovanja);
            return u;

        }

        public void OperatorToClientSendBill(string suma,string klijentIP,string klijentPort)
        {
            Common.Client cli = new Common.Client();
            IClientConnection klijentProxy = cli.GetClientProxy();//ova metoda treba da prima klijentIP i klijentPort
            klijentProxy.SendBill(suma);
        }

        public Racun ClientToBankKreirajRacun(Racun r)
        {
            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }
            return bankProxy.KreirajRacun(r);
        }

        public bool ClientToBankObrisiRacun(string brojRacuna)
        {
            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }
            return bankProxy.ObrisiRacun(brojRacuna);
        }

        public bool ClientAndOperatorToBankSetIpAndPort(string username, string ip, string port)
        {
            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }
            return bankProxy.SetIpAndPort(username, ip, port);
        }

        public bool BankToOperatorNotifyRacunAdded(Racun r, string operatorIp, string operatorPort)
        {
            Common.Client cli = new Common.Client();
            
            IOperatorConnection operatorProxy = cli.GetOperatorProxy(operatorIp, operatorPort);
            operatorProxy.NotifyRacunAdded(r);
            return true;
        }

        public Racun ClientToBankUzmiKlijentskiRacun(string username)
        {
            if (bankProxy == null)
            {
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
            }
            return bankProxy.UzmiKlijentskiRacun(username);
        }

        public bool BankToOperatorUpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, string suma, string operatorIp, string operatorPort)
        {
            Common.Client cli = new Common.Client();
            IOperatorConnection operatorProxy = cli.GetOperatorProxy(operatorIp, operatorPort);
            operatorProxy.UpdateStatus(korisnikKojiJeUplatio, operaterKomeJeUplaceno, suma);
            return true;
        }
    }
}
