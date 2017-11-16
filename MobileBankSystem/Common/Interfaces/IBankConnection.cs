using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IBankConnection
    {
        /// <summary>
        /// Metoda koju poziva admin prilikom dodavanja novog racuna u banci
        /// </summary>
        [OperationContract]
        void AddAccount();

        /// <summary>
        /// Metoda kojom se sa jednog racuna prebacuje novac na drugi
        /// </summary>
        [OperationContract]
        void Transfer();
    }
}
