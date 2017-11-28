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
        private Dictionary<string, Instance> CertDBClients = new Dictionary<string, Instance>();
        private Dictionary<string, Instance> CertDBOperaters = new Dictionary<string, Instance>();

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

            return bankProxy.AddAccount(u,mode);
            
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
            Instance i = CertDBClients[klijentIP];
            Client<IClientConnection> cli = new Client<IClientConnection>(i.CN, i.IpAddress, i.Port, "ClientConnection");
            IClientConnection klijentProxy = cli.GetProxy();//ova metoda treba da prima klijentIP i klijentPort

            // Treba GatewayLogger

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
            // Ne treba GatewayLogger

            return bankProxy.UzmiKlijentskiRacun(username);
        }

        public bool BankToOperatorUpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, string suma, string operatorIp, string operatorPort)
        {
            Instance i = CertDBOperaters[operatorIp];
            Client<IOperatorConnection> cli = new Client<IOperatorConnection>(i.CN, i.IpAddress, i.Port, "OperaterConnection");
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
            // Ne treba GatewayLogger

            return bankProxy.ShutdownOperator(username);
        }

        public bool ClientToBankShutdownClient(string username)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            // Ne treba GatewayLogger

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
            // Ne treba GatewayLogger

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
            Instance i = CertDBOperaters[ip];
            Client<IOperatorConnection> cli = new Client<IOperatorConnection>(i.CN, i.IpAddress, i.Port, "OperaterConnection");
            IOperatorConnection operatorProxy = cli.GetProxy();

            // Treba GatewayLogger

            return operatorProxy.AddRacun(racun);
        }

        public User ClientToBankGetOperator(string operatorName)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            // Ne treba GatewayLogger

            return bankProxy.GetOperator(operatorName);
        }

        public User OperatorToBankGetClient(string clientUsername)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            // Ne treba GatewayLogger
            return bankProxy.GetClient(clientUsername);

        }

        public bool CheckIntoGateway(string ip, string port, string role) // role = WindowIdentity.getCurrent().Name  --> mbclient_1
        {
            if (role.Contains("operator"))
            {
                if (CertDBOperaters.ContainsKey(ip))
                {
                    return false;
                }
                CertDBOperaters.Add(ip, new Instance(ip,port,role));
            }
            else if (role.Contains("client"))
            {
                if (CertDBClients.ContainsKey(ip))
                {
                    return false;
                }
                CertDBClients.Add(ip, new Instance(ip,port,role));
            }

            foreach(KeyValuePair<string, Instance> x in CertDBClients)
            {
                Console.WriteLine("{0} {1} {2}", x.Value.IpAddress, x.Value.Port, x.Value.CN);
            }

            foreach (KeyValuePair<string, Instance> x in CertDBOperaters)
            {
                Console.WriteLine("{0} {1} {2}", x.Value.IpAddress, x.Value.Port, x.Value.CN);
            }

            return true;
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
