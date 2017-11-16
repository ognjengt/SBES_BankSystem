using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operator
{
    public class OperaterConnection : IOperatorConnection
    {
        public void AddClient()
        {
            Console.WriteLine("Client added");
        }

        public void UpdateStatus()
        {
            Console.WriteLine("StatusUpdated");
        }
    }
}
