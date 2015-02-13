using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_UnitTestingExcercise
{
    public class AverageJoeBankService : IAverageJoeBankService
    {
        public bool Authenticate(string userName, string password)
        {
            // This is just simulate the time taken for the web service to authenticate! 
            System.Threading.Thread.Sleep(5000);
            return true;
        }
    }
}
