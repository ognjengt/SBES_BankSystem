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
        /// Admin dodaje novog klijenta za operatera
        /// </summary>
        [OperationContract]
        void AddClient();

        /// <summary>
        /// Primanje podataka od banke, tj kada se novac uploaduje na racun, banka javlja operatoru da poveca
        /// </summary>
        [OperationContract]
        void UpdateStatus();
    }
}
