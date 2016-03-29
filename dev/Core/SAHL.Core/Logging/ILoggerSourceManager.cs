using System;
using System.Collections.Generic;

namespace SAHL.Core.Logging
{
    public interface ILoggerSourceManager
    {
        void RegisterSource(ILoggerSource loggerSource);

        void UnregisterSource(ILoggerSource loggerSource);

        IDictionary<Guid, ILoggerSource> AvailableSources { get; }
    }
}