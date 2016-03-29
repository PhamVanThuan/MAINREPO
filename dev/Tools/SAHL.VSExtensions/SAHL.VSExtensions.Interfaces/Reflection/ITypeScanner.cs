using System.Collections.Generic;

namespace SAHL.VSExtensions.Interfaces.Reflection
{
    public interface ITypeScanner
    {
        void Scan(IEnumerable<IScannableAssembly> assemblies, IEnumerable<IScanConvention> scanConventions);
    }
}