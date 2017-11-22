using CertManager;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    //temlate klasa klijenta gde ce klijent koristiti interfejs INTERFACE koji je prosledjen prilikom poziva konsturktora
    public class Client<INTERFACE>
    {
        INTERFACE proxy;                    //vracam u funkciji getProxy kako bi klijent mogao da pristupi serverskim metodama
        ChannelFactory<INTERFACE> factory;  //kanal koji ce da kreira proxy

        /*
            CN - naziv sertifikata gde se nalazi javni kljuc servica
            ip, port na koje se nalazi servis
        */
        public Client(string CN, string ip, string port)
        {
            string srvCertCN = CN;  //ime servsa - ujedno i naziv njegovog cert.

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate; //auth se vrsi pomocu cert.

            //iz foldera trusted people uzima javni kljuc serverskog cert.
            X509Certificate2 srvCert = Manager.GetCertificateFormStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            //endpoing koji client treba da pogodi. sastoji se od uri-ja kao prvog param. i drugog param. - javnog kljuca iz cert koji smo gore uzeli. taj ljkuc nam kaze
            //da na serveru treba da nas ocekuje cert koji pored tog javnog kljuca ima i neki svoj privatni
            EndpointAddress address = new EndpointAddress(new Uri(String.Format("net.tcp://{0}:{1}/Service", ip, port)), new X509CertificateEndpointIdentity(srvCert));
            //kreiramo kanal
            factory = new ChannelFactory<INTERFACE>(binding, address);
            //izvlacimo nase klijentsko ime i nas sertifikat
            string cliCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            //nacin auth.
            factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            //poojma nemam
            factory.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            //iz personal foldera uzimamo nas sertifikat sa privatnim i javnim kljucem
            factory.Credentials.ClientCertificate.Certificate = Manager.GetCertificateFormStorage(StoreName.My, StoreLocation.LocalMachine, cliCertCN);
            //kreiramo proxy
            proxy = factory.CreateChannel();
        }

        public INTERFACE GetProxy()
        {
            //vracam proxy
            return proxy;
        }

        public void Dispose()
        {
            //gasi
            factory.Close();
        }
        /*
        /// <summary>
        /// Vraca proxy za komuniciranje sa bankom
        /// </summary>
        /// <returns></returns>
        public IBankConnection GetBankProxy()
        {
            string cliCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            var binding = new NetTcpBinding();
            binding.TransactionFlow = true;
            ChannelFactory<IBankConnection> factory = new ChannelFactory<IBankConnection>(binding,
            new EndpointAddress("net.tcp://localhost:65000/BankConnection"));
            IBankConnection proxy = factory.CreateChannel();

            return proxy;
        }

        /// <summary>
        /// Vraca proxy za komuniciranje sa klijentom
        /// </summary>
        /// <returns></returns>
        public IClientConnection GetClientProxy(string ip, string port)
        {
            // Skontati kako da uzmemo port bas od tog klijenta koji je poslao zahtev, posto ne mozemo slati fiksno na 62000
            var binding = new NetTcpBinding();
            binding.TransactionFlow = true;
            ChannelFactory<IClientConnection> factory = new ChannelFactory<IClientConnection>(binding,
            new EndpointAddress(String.Format("net.tcp://{0}:{1}/ClientConnection", ip, port)));
            IClientConnection proxy = factory.CreateChannel();

            return proxy;
        }

        /// <summary>
        /// Vraca proxy za komuniciranje sa SOA Gateway-om
        /// </summary>
        /// <returns></returns>
        public IGatewayConnection GetGatewayProxy()
        {
            var binding = new NetTcpBinding();
            binding.TransactionFlow = true;
            ChannelFactory<IGatewayConnection> factory = new ChannelFactory<IGatewayConnection>(binding,
            new EndpointAddress("net.tcp://localhost:63000/GatewayConnection"));
            IGatewayConnection proxy = factory.CreateChannel();

            return proxy;
        }

        /// <summary>
        /// Vraca proxy za komuniciranje sa Operatorom
        /// </summary>
        /// <returns></returns>
        public IOperatorConnection GetOperatorProxy(string ip, string port)
        {
            var binding = new NetTcpBinding();
            binding.TransactionFlow = true;
            ChannelFactory<IOperatorConnection> factory = new ChannelFactory<IOperatorConnection>(binding,
            new EndpointAddress(String.Format("net.tcp://{0}:{1}/OperaterConnection", ip, port)));
            IOperatorConnection proxy = factory.CreateChannel();

            return proxy;
            
        }
        */
    }
}
