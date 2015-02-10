using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FirstAppwithTesting
{
    public interface InterfaceToTest
    {
        double Add(double firstOp, double secondOp);
        double Subtract(double firstOp, double secondOp);

    }
}
