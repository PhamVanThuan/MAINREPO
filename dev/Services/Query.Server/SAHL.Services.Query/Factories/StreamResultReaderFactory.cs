using System;
using System.IO;
using System.Net;

namespace SAHL.Services.Query.Factories
{
    public class StreamResultReaderFactory : IStreamResultReaderFactory
    {
        public T Process<T>(WebRequest request, Func<StreamReader, T> actionToPerformOnStreamReader)
        {
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream ?? Stream.Null))
            {
                return actionToPerformOnStreamReader(reader);
            }
        }
    }
}