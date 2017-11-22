using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CertManager;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gateway
{
    public class GatewayServer : Server
    {
        public override void Close()
        {
            serviceHost.Close();
        }

        public override void Start()
        {
            string srvCertName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:63000/GatewayConnection";

            serviceHost = new ServiceHost(typeof(GatewayConnection));
            serviceHost.AddServiceEndpoint(typeof(IGatewayConnection), binding, address);
            serviceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            serviceHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            serviceHost.Credentials.ServiceCertificate.Certificate =
                    Manager.GetCertificateFormStorage(StoreName.My, StoreLocation.LocalMachine, srvCertName);

            serviceHost.Open();
            this.ipAddress = "localhost"; // kasnije izmeniti
            this.port = 63000;
        }
    }
}
