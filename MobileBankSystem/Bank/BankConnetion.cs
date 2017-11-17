using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankConnetion : IBankConnection
    {
        public void AddAccount()
        {
            Console.WriteLine("AddAccount called");
        }

        public bool CheckLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void Transfer()
        {
            Console.WriteLine("Trasfer called");
        }
        
    }
}
