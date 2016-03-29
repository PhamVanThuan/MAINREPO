using System;
using System.IO;
using System.Net;

namespace SAHL.Services.Query.Factories
{
    public interface IStreamResultReaderFactory
    {
        T Process<T>(WebRequest request, Func<StreamReader, T> actionToPerformOnStreamReader);
    }
}