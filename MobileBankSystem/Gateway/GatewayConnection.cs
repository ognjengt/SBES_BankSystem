using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        public bool ClientToBankAddAccount(User u,int mode)
        {
            //pozovem metodu iz banke
            //kacimo se na banku, uplacujemo novac
            if (bankProxy == null)
            {
                // Kod svih konekcija na banku mora se uzeti iz baze na kojem ip-u je banka, tako sto ce i banka i gateway i svaki operater da se upise u globalnu bazu da se zna na cemu slusaju i gde ce se ostali konektovati
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("AddAccount", "Bank");

            // Isto i za Inicijatore, ali to kasnije kad dodju sertifikati
            var stackFrame = new StackFrame(1);
            var callingMethod = stackFrame.GetMethod();
            var callingClass = callingMethod.DeclaringType;
            // uzeti username od usera koji poziva i pozvati GatewayLogger.AddInicijator(username);

            bool a=bankProxy.AddAccount(u,mode);
            if (a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ClientToBankTransfer(string brojKlijentskogRacuna, string brojOperatorskogRacuna, string korisnikKojiVrsiTransfer, string value)
        {
            bool retVal;

            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }

            GatewayLogger.AddMethod("Transfer", "Bank");

            retVal = bankProxy.Transfer(brojKlijentskogRacuna, brojOperatorskogRacuna, korisnikKojiVrsiTransfer, value);
            return retVal;
        }

        public User ClientToBankCheckLogin(string username, string password, string nacinLogovanja)
        {
            
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("CheckLogin", "Bank");

            User u =bankProxy.CheckLogin(username, password, nacinLogovanja);
            return u;

        }

        public void OperatorToClientSendBill(string suma,string klijentIP,string klijentPort)
        {
            Client<IClientConnection> cli = new Client<IClientConnection>("mbclient_1", klijentIP, klijentPort, "ClientConnection");
            IClientConnection klijentProxy = cli.GetProxy();//ova metoda treba da prima klijentIP i klijentPort
            klijentProxy.SendBill(suma);
        }

        public Racun ClientToBankKreirajRacun(Racun r)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("KreirajRacun", "Bank");

            return bankProxy.KreirajRacun(r);
        }

        public bool ClientToBankObrisiRacun(string brojRacuna)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("ObrisiRacun", "Bank");

            return bankProxy.ObrisiRacun(brojRacuna);
        }

        public bool ClientAndOperatorToBankSetIpAndPort(string username, string ip, string port)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("SetIpAndPort", "Bank");

            return bankProxy.SetIpAndPort(username, ip, port);
        }

        //public bool BankToOperatorNotifyRacunAdded(Racun r, string operatorIp, string operatorPort)
        //{
        //    // trenutno salje ne sifrovane podatke, videti da li treba sifrovati IP i PORT na svim ovakvim metodama
        //    Client<IOperatorConnection> cli = new Client<IOperatorConnection>("mboperator_1", operatorIp, operatorPort, "OperatorConnection");
            
        //    IOperatorConnection operatorProxy = cli.GetProxy();
        //    operatorProxy.NotifyRacunAdded(r);
        //    return true;
        //}

        public Racun ClientToBankUzmiKlijentskiRacun(string username)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("UzmiKlijentskiRacun", "Bank");

            return bankProxy.UzmiKlijentskiRacun(username);
        }

        public bool BankToOperatorUpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, string suma, string operatorIp, string operatorPort)
        {
            Client<IOperatorConnection> cli = new Client<IOperatorConnection>("mboperator_1", operatorIp, operatorPort, "OperaterConnection");
            IOperatorConnection operatorProxy = cli.GetProxy();
            operatorProxy.UpdateStatus(korisnikKojiJeUplatio, operaterKomeJeUplaceno, suma);
            return true;
        }

        public bool ClientToBankSetIpAndPortClient(string username, string ip, string port)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("SetIpAndPortClient", "Bank");

            return bankProxy.SetIpAndPortClient(username, ip, port);
        }

        public bool OperatorToBankShutdownOperator(string username)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("ShutdownOperator", "Bank");

            return bankProxy.ShutdownOperator(username);
        }

        public bool ClientToBankShutdownClient(string username)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("ShutdownClient", "Bank");

            return bankProxy.ShutdownClient(username);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string OperatorToBankGetOperatorsClients(string operatorUsername)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("ObrisiRacun", "Bank");

            return bankProxy.GetOperatorsClients(operatorUsername);
        }

        public bool ClientToBankIzmeniRacun(Racun r)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("IzmeniRacun", "Bank");

            return bankProxy.IzmeniRacun(r);
        }

        public bool ClientToOperatorAddRacun(Racun racun,string ip, string port)
        {
            Client<IOperatorConnection> cli = new Client<IOperatorConnection>("mboperator_1", ip, port, "OperaterConnection");
            IOperatorConnection operatorProxy = cli.GetProxy();
            return operatorProxy.AddRacun(racun);
        }

        public User ClientToBankGetOperator(string operatorName)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            GatewayLogger.AddMethod("GetOperator", "Bank");

            return bankProxy.GetOperator(operatorName);
        }
        //public bool BankToOperatorNotifyRacunDeleted(Racun r, string operatorIp, string operatorPort)
        //{
        //    // trenutno salje ne sifrovane podatke, videti da li treba sifrovati IP i PORT na svim ovakvim metodama
        //    Client<IOperatorConnection> cli = new Client<IOperatorConnection>("mboperator_1", operatorIp, operatorPort, "OperatorConnection");

        //    IOperatorConnection operatorProxy = cli.GetProxy();
        //    operatorProxy.NotifyRacunDeleted(r);
        //    return true;
        //}

        //public bool BankToOperatorNotifyRacunChanged(Racun r, string operatorIp, string operatorPort)
        //{
        //    // trenutno salje ne sifrovane podatke, videti da li treba sifrovati IP i PORT na svim ovakvim metodama
        //    Client<IOperatorConnection> cli = new Client<IOperatorConnection>("mboperator_1", operatorIp, operatorPort, "OperatorConnection");

        //    IOperatorConnection operatorProxy = cli.GetProxy();
        //    operatorProxy.NotifyRacunChanged(r);
        //    return true;
        //}
    }
}
