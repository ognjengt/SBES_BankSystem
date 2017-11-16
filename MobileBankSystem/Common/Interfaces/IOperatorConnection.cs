using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IOperatorConnection
    {
        /// <summary>
        /// admin dodaje novog klijenta za operatera
        /// </summary>
        [OperationContract]
        void AddClient();
        [OperationContract]
        void UpdateStatus();
    }
}
