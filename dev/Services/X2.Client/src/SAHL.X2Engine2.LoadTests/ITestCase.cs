using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.LoadTests
{
    public interface ITestCase
    {
        void Test(string hostName, int workerId);
    }
}
