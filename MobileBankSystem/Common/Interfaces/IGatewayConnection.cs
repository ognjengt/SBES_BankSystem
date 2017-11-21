using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IGatewayConnection
    {
        /// <summary>
        /// Poziva metodu AddAccount na banci
        /// </summary>
        [OperationContract]
        void ClientToBankAddAccount(User u);

        /// <summary>
        /// Poziva metodu KreirajRacun na banci
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        [OperationContract]
        Racun ClientToBankKreirajRacun(Racun r);

        /// <summary>
        /// Poziva metodu ObrisiRacun na banci
        /// </summary>
        /// <param name="brojRacuna"></param>
        /// <returns></returns>
        [OperationContract]
        bool ClientToBankObrisiRacun(string brojRacuna);

        /// <summary>
        /// Poziva metodu za transfer novca na banci
        /// </summary>
        /// <param name="myUsername"></param>
        /// <param name="myUsernameOnOperator"></param>
        /// <param name="operatorUsername"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        bool ClientToBankTransfer(string myUsername, string myUsernameOnOperator, string operatorUsername, string value);

        /// <summary>
        /// Poziva metodu CheckLogin na banci
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        User ClientToBankCheckLogin(string username, string password, string nacinLogovanja);

        /// <summary>
        /// Poziva metodu SetIpAndPort na banci
        /// </summary>
        /// <param name=""></param>
        /// <param name="BankToOperator"></param>
        [OperationContract]
        bool ClientAndOperatorToBankSetIpAndPort(string username, string ip, string port);

        [OperationContract]
        Racun ClientToBankUzmiKlijentskiRacun(string username);

        /// <summary>
        /// Preusmerava saobracaj od banke ka operateru
        /// </summary>
        [OperationContract]
        void BankToOperator();

        /// <summary>
        /// Preusmerava saobracaj od Operatera do klijenta
        /// </summary>
        [OperationContract]
        void OperatorToClientSendBill(string suma, string klijentIP, string klijentPort);

        /// <summary>
        /// Poziva UpdateStatus na operateru
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool BankToOperatorUpdateStatus(string korisnikKojiJeUplatio, string operaterKomeJeUplaceno, string suma, string operatorIp, string operatorPort);

        /// <summary>
        /// Poziva NotifyRacunAdded na operateru
        /// </summary>
        /// <param name="r"></param>
        [OperationContract]
        bool BankToOperatorNotifyRacunAdded(Racun r, string operatorIp, string operatorPort);
    }
}
