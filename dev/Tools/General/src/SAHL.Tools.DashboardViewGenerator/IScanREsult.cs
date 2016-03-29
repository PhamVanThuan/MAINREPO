using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator
{
    public interface IScanResult
    {
        IList<TypeDefinition> Types { get; }
    }
}
