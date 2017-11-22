using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IBankConnection : IDisposable
    {
        /// <summary>
        /// Metoda koju poziva admin prilikom dodavanja novog racuna u banci
        /// </summary>
        [OperationContract]
        void AddAccount(User u,int mode);

        /// <summary>
        /// Kreira racun za fizicka ili pravna lica
        /// </summary>
        /// <param name="r"></param>
        [OperationContract]
        Racun KreirajRacun(Racun r);

        /// <summary>
        /// Brise racun iz baze
        /// </summary>
        /// <param name="brRacuna"></param>
        /// <returns></returns>
        [OperationContract]
        bool ObrisiRacun(string brRacuna);

        /// <summary>
        /// Metoda kojom se sa jednog racuna prebacuje novac na drugi
        /// </summary>
        [OperationContract]
        bool Transfer(string brojKlijentskogRacuna, string brojOperatorskogRacuna, string korisnikKojiVrsiTransfer, string value);

        /// <summary>
        /// Vraca usera ili null u zavisnosti da li postoji u sistemu ili ne
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        User CheckLogin(string username,string password, string nacinLogovanja);


        [OperationContract]
        Racun UzmiKlijentskiRacun(string username);

        /// <summary>
        /// Postavlja ip i port podignutog operatera u bazu
        /// </summary>
        /// <param name="u"></param>
        [OperationContract]
        bool SetIpAndPort(string username, string ip, string port);

        /// <summary>
        /// Postavlja ip i port podignutog klijenta u bazu
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetIpAndPortClient(string username, string ip, string port);

        /// <summary>
        /// Brise operatora sa ovim usernameom iz liste aktivnih operatora
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [OperationContract]
        bool ShutdownOperator(string username);

        /// <summary>
        /// Brise klijenta sa ovim usernameom iz liste aktivnih klijenata
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [OperationContract]
        bool ShutdownClient(string username);



    }
}
