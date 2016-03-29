using System.Collections.Generic;

namespace SAHL.Tools.RestServiceRoutenator
{
    public interface ITypeScanner
    {
        IEnumerable<IScanResult> Scan(string desiredAssemblyPath, out string assemblyName);
    }
}