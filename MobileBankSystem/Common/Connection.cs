using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public class Connection : IConnection
    {
        [OperationContract]
        public void TestConnection(string testNeki)
        {
            Console.WriteLine(testNeki);
        }
    }
}
