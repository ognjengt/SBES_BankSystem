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

        public void ClientToBankAddAccount(User u,int mode)
        {
            //pozovem metodu iz banke
            //kacimo se na banku, uplacujemo novac
            if (bankProxy == null)
            {
                // Kod svih konekcija na banku mora se uzeti iz baze na kojem ip-u je banka, tako sto ce i banka i gateway i svaki operater da se upise u globalnu bazu da se zna na cemu slusaju i gde ce se ostali konektovati
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("AddAccount"))
            {
                Metoda m = new Metoda("AddAccount", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["AddAccount"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

            // Isto i za Inicijatore, ali to kasnije kad dodju sertifikati

            bankProxy.AddAccount(u,mode);
        }

        public bool ClientToBankTransfer(string brojKlijentskogRacuna, string brojOperatorskogRacuna, string korisnikKojiVrsiTransfer, string value)
        {
            bool retVal;

            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }

            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("Transfer"))
            {
                Metoda m = new Metoda("Transfer", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["Transfer"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

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
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("CheckLogin"))
            {
                Metoda m = new Metoda("CheckLogin", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["CheckLogin"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

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
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("KreirajRacun"))
            {
                Metoda m = new Metoda("KreirajRacun", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["KreirajRacun"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

            return bankProxy.KreirajRacun(r);
        }

        public bool ClientToBankObrisiRacun(string brojRacuna)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("ObrisiRacun"))
            {
                Metoda m = new Metoda("ObrisiRacun", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["ObrisiRacun"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

            return bankProxy.ObrisiRacun(brojRacuna);
        }

        public bool ClientAndOperatorToBankSetIpAndPort(string username, string ip, string port)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("SetIpAndPort"))
            {
                Metoda m = new Metoda("SetIpAndPort", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["SetIpAndPort"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

            return bankProxy.SetIpAndPort(username, ip, port);
        }

        public bool BankToOperatorNotifyRacunAdded(Racun r, string operatorIp, string operatorPort)
        {
            // trenutno salje ne sifrovane podatke, videti da li treba sifrovati IP i PORT na svim ovakvim metodama
            Client<IOperatorConnection> cli = new Client<IOperatorConnection>("mboperator_1", operatorIp, operatorPort, "OperatorConnection");
            
            IOperatorConnection operatorProxy = cli.GetProxy();
            operatorProxy.NotifyRacunAdded(r);
            return true;
        }

        public Racun ClientToBankUzmiKlijentskiRacun(string username)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("UzmiKlijentskiRacun"))
            {
                Metoda m = new Metoda("UzmiKlijentskiRacun", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["UzmiKlijentskiRacun"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

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
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("SetIpAndPortClient"))
            {
                Metoda m = new Metoda("SetIpAndPortClient", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["SetIpAndPortClient"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

            return bankProxy.SetIpAndPortClient(username, ip, port);
        }

        public bool OperatorToBankShutdownOperator(string username)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("ShutdownOperator"))
            {
                Metoda m = new Metoda("ShutdownOperator", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["ShutdownOperator"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();
            return bankProxy.ShutdownOperator(username);
        }

        public bool ClientToBankShutdownClient(string username)
        {
            if (bankProxy == null)
            {
                Client<IBankConnection> cli = new Client<IBankConnection>("mbbank", Konstante.BANK_IP, Konstante.BANK_PORT.ToString(), "BankConnection");
                bankProxy = cli.GetProxy();
            }
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("ShutdownClient"))
            {
                Metoda m = new Metoda("ShutdownClient", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["ShutdownClient"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();
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
            if (!GatewayLogger.BazaStatistikeMetoda.ContainsKey("ObrisiRacun"))
            {
                Metoda m = new Metoda("ObrisiRacun", 0, "Bank");
                GatewayLogger.BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            GatewayLogger.BazaStatistikeMetoda["ObrisiRacun"].BrojPoziva++;
            GatewayLogger.SacuvajStatistikuMetoda();

            return bankProxy.GetOperatorsClients(operatorUsername);
        }
    }
}
