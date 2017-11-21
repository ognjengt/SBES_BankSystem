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
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
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
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
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
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
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
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
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
                Common.Client cli = new Common.Client();
                bankProxy = cli.GetBankProxy();
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
            Common.Client cli = new Common.Client();
            IOperatorConnection operatorProxy = cli.GetOperatorProxy(operatorIp, operatorPort);
            operatorProxy.UpdateStatus(korisnikKojiJeUplatio, operaterKomeJeUplaceno, suma);
            return true;
        }
    }
}
