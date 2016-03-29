using System.Collections.Generic;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface ITypeScanner
    {
        IEnumerable<IScanResult> Scan(string input, out string assemblyName);
    }
}