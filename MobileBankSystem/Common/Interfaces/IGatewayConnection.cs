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
        /// Preusmerava saobracaj od klijenta do banke, tj komunicira sa bankom i salje joj klijentske zahteve
        /// </summary>
        [OperationContract]
        void ClientToBank();

        /// <summary>
        /// Preusmerava saobracaj od banke ka operateru
        /// </summary>
        [OperationContract]
        void BankToOperator();

        /// <summary>
        /// Preusmerava saobracaj od Operatera do klijenta
        /// </summary>
        [OperationContract]
        void OperatorToClient();
    }
}
