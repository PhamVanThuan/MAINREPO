using System;

namespace SAHL.Core.IoC
{
    public interface ITypeScannerConvention
    {
        bool Process(Type typeToProcess);
    }
}