using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IClientConnection
    {
        /// <summary>
        /// Metoda koja na svaka 2 minuta salje klijentu njegov "mesecni" racun
        /// </summary>
        [OperationContract]
        void SendBill();

    }
}
