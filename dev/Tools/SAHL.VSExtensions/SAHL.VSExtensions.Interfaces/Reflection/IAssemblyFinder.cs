using System;
using System.Collections.Generic;

namespace SAHL.VSExtensions.Interfaces.Reflection
{
    public interface IAssemblyFinder
    {
        IEnumerable<IScannableAssembly> Where(Func<string, bool> function);
    }
}