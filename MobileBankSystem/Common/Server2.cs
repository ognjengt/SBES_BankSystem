using CertManager;
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
    public class Server2<INTERFACE>
    {
        ServiceHost host;
        //ip, port na kome je server
        //serviceName je treci parametar u string.format prilikom kreiranja adrese
        //Type typeofsrcClass povratna vrednost typeof(npr. ClientConnection) -> kaze u kojoj klasi su iplementirane
        //  metode iz interfejsa INTERFACE
        public Server2(string ip, string port, string serviceName, Type typeOfSrcClass)
        {
            //ime naseg window usera, ujedno i naseg cert.
            string srvCertName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            //tip auth
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = String.Format("net.tcp://{0}:{1}/{2}", ip, port, serviceName);

            host = new ServiceHost(typeOfSrcClass);
            host.AddServiceEndpoint(typeof(INTERFACE), binding, address);
            //nacin auth
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            //uzimamo nas cert
            host.Credentials.ServiceCertificate.Certificate =
                Manager.GetCertificateFormStorage(StoreName.My, StoreLocation.LocalMachine, srvCertName);

            host.Open();
        }

        public void Close()
        {
            host.Close();
        }
    }
}
