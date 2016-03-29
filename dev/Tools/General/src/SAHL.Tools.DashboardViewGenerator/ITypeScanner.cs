using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator
{
    public interface ITypeScanner
    {
        IEnumerable<IScanResult> Scan(string input, out string assemblyName);
    }
}
